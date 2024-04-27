using System.Collections.Concurrent;
using System.Threading.Channels;
using Domain.Models;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class ReaderService(
    IProductStatRepository productStatRepository,
    ILogger<ReaderService> logger) : IDemandReaderService
{
    private int _readNumber;
    private SemaphoreSlim? _semaphore;
    private readonly ConcurrentBag<Task> _tasks = [];

    public async Task ReadProductStats
    (
        CancellationToken cancellationToken, 
        ManualResetEvent conditionEvent, 
        SemaphoreSlim semaphore, 
        Channel<ProductStat> productStatChannel
    )
    {
        try
        {
            _semaphore = semaphore;
            logger.LogInformation("Start reading product stats");
            foreach (var productStat in productStatRepository.GetProductStats())
            {
                logger.LogInformation("Read product stat: {@ProductStat}", productStat);
                cancellationToken.ThrowIfCancellationRequested();
                conditionEvent.WaitOne();
                await _semaphore.WaitAsync(cancellationToken);
                _tasks.Add(Task.Run(() => productStatChannel.Writer.WriteAsync(productStat, cancellationToken),
                    cancellationToken));
                Interlocked.Increment(ref _readNumber);
                logger.LogInformation("Read number: {@ReadNumber}", _readNumber);
                logger.LogInformation("Sent product stat for processing: {@ProductStat}", productStat);
            }

            await Task.WhenAll(_tasks);
            productStatChannel.Writer.Complete();
            logger.LogInformation("Completed sending product stats");
        } 
        catch (OperationCanceledException)
        {
            logger.LogInformation("Calculate records operation was cancelled");
        }
        catch (Exception e)
        {
            logger.LogCritical(e, "Cannot read demands");
        }
        finally
        {
            await Task.WhenAll(_tasks);
        }
    }


    public async Task ChangeSemaphore(SemaphoreSlim newSemaphore)
    {
        await Task.WhenAll(_tasks);
        _tasks.Clear();
        
        _semaphore = newSemaphore;
    }
}