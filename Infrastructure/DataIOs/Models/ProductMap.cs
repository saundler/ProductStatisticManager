using CsvHelper.Configuration;
using Domain.Models;

namespace Infrastructure.DataIOs.Models;

public sealed class ProductMap : ClassMap<ProductStat>
{
    public ProductMap()
    {
        Map(m => m.Id).Name("id");
        Map(m => m.Prediction).Name("prediction");
        Map(m => m.Stock).Name("stock");
    }
}
