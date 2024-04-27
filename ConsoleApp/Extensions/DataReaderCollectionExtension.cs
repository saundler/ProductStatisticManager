using ConsoleApp.Settings;
using Domain.Models;
using Infrastructure.DataIOs;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp.Extensions;

public static class DataReaderCollectionExtension
{
    public static IServiceCollection AddDataReaders(this IServiceCollection serviceCollection, ApplicationSettings settings)
    {
        serviceCollection.AddSingleton<IDataReader<ProductStat>>(_ =>
            new ProductStatFileReader(settings.CsvSettings.InputPath, settings.CsvSettings.Separator));
        
        return serviceCollection;
    }
}