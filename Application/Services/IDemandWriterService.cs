using System.Threading.Channels;
using Domain.Models;

namespace Application.Services;

public interface IDemandWriterService
{
    Task SaveDemands
    (
        CancellationToken cancellationToken,
        Channel<ProductDemand> productDemandChannel
    );
}