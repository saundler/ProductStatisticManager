using ConsoleApp.Settings;
using Domain.Models;
using Infrastructure.DataIOs;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp.Extensions;

public static class DataWriterCollectionExtension
{
    public static IServiceCollection AddDataWriters(this IServiceCollection serviceCollection, ApplicationSettings settings)
    {
        serviceCollection.AddSingleton<IDataWriter<ProductDemand>>(_ =>
            new ProductDemandFileWriter(settings.CsvSettings.OutputPath, settings.CsvSettings.Separator));
        
        return serviceCollection;
    }
}