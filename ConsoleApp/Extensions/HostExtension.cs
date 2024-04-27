using Application.Services;
using Microsoft.Extensions.Hosting;

namespace ConsoleApp.Extensions;

public static class HostExtension
{
    public static async Task StartApplication(this IHost host, IDemandProcessingService service, CancellationToken cancellationToken)
    {
        await Task.WhenAll(host.RunAsync(cancellationToken), service.ProcessDemands(cancellationToken));
    }
}