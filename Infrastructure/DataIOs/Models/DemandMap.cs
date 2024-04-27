using CsvHelper.Configuration;
using Domain.Models;

namespace Infrastructure.DataIOs.Models;

public sealed class DemandMap : ClassMap<ProductDemand>
{
    public DemandMap()
    {
        Map(m => m.Id).Name("id");
        Map(m => m.Demand).Name("demand");
    }
}