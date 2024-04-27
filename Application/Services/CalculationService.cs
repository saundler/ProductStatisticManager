using System.Collections.Concurrent;
using System.Threading.Channels;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public sealed class CalculationService : IDemandCalculationService
{
    private SemaphoreSlim? _semaphore;
    private readonly ConcurrentBag<Task> _tasks = [];
    private static ILogger<CalculationService> _logger;
    
    public CalculationService(ILogger<CalculationService> logger)
    {
        _logger = logger;
    }
    
    private static async Task<ProductDemand> CalculateDemand(ProductStat product)
    {
        _logger.LogInformation("Start calculating demand for product: {@Product}", product);
        
        await Task.Delay(1000);
        
        _logger.LogInformation("Finish calculating demand for product: {@Product}", product);

        return new ProductDemand(product.Id, Math.Max(product.Prediction - product.Stock, 0));
    }

    public async Task CalculateRecords
    (
        CancellationToken cancellationToken,
        Channel<ProductStat> productStatChannel, 
        Channel<ProductDemand> demandChannel,
        SemaphoreSlim semaphore
    )
    {
        try
        {
            _logger.LogInformation("Start calculating demands");
            _semaphore = semaphore;
            await foreach (var productStat in productStatChannel.Reader.ReadAllAsync(cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();
                _tasks.Add(CalculateAndWrite(productStat, cancellationToken, demandChannel));
            }

            _logger.LogInformation("Completed calculating demands");
            await Task.WhenAll(_tasks);
            demandChannel.Writer.Complete();
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Calculate demands operation was cancelled");
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "Cannot calculate demands");
        }
        finally
        {
            await Task.WhenAll(_tasks);
        }
    }

    private async Task CalculateAndWrite
    (
        ProductStat productStat, 
        CancellationToken cancellationToken, 
        Channel<ProductDemand> demandChannel
    )
    {
        try
        {
            _logger.LogInformation($"Read {productStat} from channel");

            var demand = await Task.Run(() => CalculateDemand(productStat), cancellationToken);

            _logger.LogInformation(
                "Calculated demand = {Demand} for product = {@ProductStat}\nSending into channel",
                demand, productStat);

            await demandChannel.Writer.WriteAsync(demand, cancellationToken);
        }
        finally
        {
            _semaphore?.Release();
        }
    }
    public async Task ChangeSemaphore(SemaphoreSlim newSemaphore)
    {
        await Task.WhenAll(_tasks);
        _semaphore = newSemaphore;
    }
}