using System.Threading.Channels;
using Domain.Models;

namespace Application.Services;

public interface IDemandReaderService
{
    Task ReadProductStats
    (
        CancellationToken cancellationToken,
        ManualResetEvent conditionEvent,
        SemaphoreSlim semaphore,
        Channel<ProductStat> productStatChannel
    );

    Task ChangeSemaphore(SemaphoreSlim newSemaphore);
}