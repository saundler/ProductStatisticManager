using System.Threading.Channels;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class DemandProcessingService(
    IDemandReaderService readerService,
    IDemandWriterService writerService,
    IDemandCalculationService calculationService,
    ILogger<IDemandProcessingService> logger,
    int maxDegreeOfParallelism) : IDemandProcessingService
{
    private SemaphoreSlim _semaphore = new(maxDegreeOfParallelism);
    private readonly ManualResetEvent _conditionEvent = new(true);


    public async Task ProcessDemands(CancellationToken cancellationToken)
    {
        var productStatChannel = Channel.CreateUnbounded<ProductStat>();
        var demandChannel = Channel.CreateUnbounded<ProductDemand>();
        var readTask = Task.Run(() =>
                readerService.ReadProductStats(cancellationToken, _conditionEvent, _semaphore, productStatChannel),
            cancellationToken);
        var calculateTask = Task.Run(() =>
                calculationService.CalculateRecords(cancellationToken, productStatChannel, demandChannel, _semaphore),
            cancellationToken);
        var writeTask = Task.Run(() => writerService.SaveDemands(cancellationToken, demandChannel), cancellationToken);

        await Task.WhenAll(readTask, calculateTask, writeTask);
    }

    public async Task MaxDegreeOfParallelismChanged(int newMaxDegreeOfParallelism)
    {
        _conditionEvent.Reset();
        logger.LogWarning("Changing max degree of parallelism to {NewMaxDegreeOfParallelism}",
            newMaxDegreeOfParallelism);
        
        _semaphore = new SemaphoreSlim(newMaxDegreeOfParallelism);
        await readerService.ChangeSemaphore(_semaphore);
        await calculationService.ChangeSemaphore(_semaphore);
        
        logger.LogWarning("Changed max degree of parallelism to {NewMaxDegreeOfParallelism}",
            newMaxDegreeOfParallelism);
        _conditionEvent.Set();
    }
}