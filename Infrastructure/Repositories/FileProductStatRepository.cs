using Domain.Models;
using Domain.Repositories;
using Infrastructure.DataIOs;

namespace Infrastructure.Repositories;

public sealed class FileProductStatRepository(IDataReader<ProductStat> dataReader) : IProductStatRepository
{
    private readonly Lazy<IEnumerable<ProductStat>> _productStats = new(dataReader.ReadData);

    public IEnumerable<ProductStat> GetProductStats() => _productStats.Value;
}