using System.Threading.Channels;
using Domain.Models;

namespace Application.Services;

public interface IDemandCalculationService
{
    Task CalculateRecords(
        CancellationToken cancellationToken,
        Channel<ProductStat> productStatChannel,
        Channel<ProductDemand> demandChannel,
        SemaphoreSlim semaphore
    );

    Task ChangeSemaphore(SemaphoreSlim newSemaphore);
}