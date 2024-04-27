using Domain.Repositories;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp.Extensions;

public static class RepositoryCollectionExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IProductDemandRepository, FileProductDemandRepository>();
        serviceCollection.AddSingleton<IProductStatRepository, FileProductStatRepository>();

        return serviceCollection;
    }
}