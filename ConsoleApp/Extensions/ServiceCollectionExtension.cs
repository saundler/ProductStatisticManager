using Application.Services;
using ConsoleApp.Settings;
using Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ConsoleApp.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection, ApplicationSettings settings)
    {
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddConsole() 
                .SetMinimumLevel(LogLevel.Information);
        });
        serviceCollection.AddSingleton<IDemandReaderService, ReaderService>();
        serviceCollection.AddSingleton<IDemandWriterService, WriterService>();
        serviceCollection.AddSingleton<IDemandCalculationService, CalculationService>();
        serviceCollection.AddSingleton<IDemandProcessingService>(provider =>
            new DemandProcessingService(
                provider.GetRequiredService<IDemandReaderService>(),
                provider.GetRequiredService<IDemandWriterService>(),
                provider.GetRequiredService<IDemandCalculationService>(),
                loggerFactory.CreateLogger<IDemandProcessingService>(),
                settings.MaxDegreeOfParallelism
            )
        );

        return serviceCollection;
    }
}