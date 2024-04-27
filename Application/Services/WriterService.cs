using System.Threading.Channels;
using Domain.Models;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public sealed class WriterService(
    IProductDemandRepository productDemandRepository,
    ILogger<WriterService> logger) : IDemandWriterService
{
    private int _writeNumber;

    public async Task SaveDemands
    (
        CancellationToken cancellationToken, 
        Channel<ProductDemand> productDemandChannel
    )
    {
        try
        {
            logger.LogInformation("Start saving demands");
            await foreach (var demand in productDemandChannel.Reader.ReadAllAsync(cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();
                Interlocked.Increment(ref _writeNumber);
                logger.LogInformation("Write number: {@WriteNumber}", _writeNumber);
                logger.LogInformation("Read demand from channel: {Demand}", demand);
                productDemandRepository.AddProductDemand(demand);
                logger.LogInformation("Saved demand: {Demand}", demand);
            }

            logger.LogInformation("Completed saving demands");
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("Save demands operation was cancelled");
        }
        catch (Exception e)
        {
            logger.LogCritical(e, "Cannot save demands");
        }
    }
}