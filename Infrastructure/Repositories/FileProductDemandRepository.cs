using Domain.Models;
using Domain.Repositories;
using Infrastructure.DataIOs;

namespace Infrastructure.Repositories;

public sealed class FileProductDemandRepository(IDataWriter<ProductDemand> dataWriter) : IProductDemandRepository
{
    public void AddProductDemand(ProductDemand productDemand)
    {
        dataWriter.WriteData(productDemand);
    }
}