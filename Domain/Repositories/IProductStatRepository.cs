using Domain.Models;

namespace Domain.Repositories;

public interface IProductStatRepository
{
    IEnumerable<ProductStat> GetProductStats();
}