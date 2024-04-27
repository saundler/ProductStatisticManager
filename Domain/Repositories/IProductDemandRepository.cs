using Domain.Models;

namespace Domain.Repositories;

public interface IProductDemandRepository
{
    void AddProductDemand(ProductDemand productDemand);
}