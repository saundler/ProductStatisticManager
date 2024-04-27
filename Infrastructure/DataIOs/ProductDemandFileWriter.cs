using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Domain.Models;
using Infrastructure.DataIOs.Models;

namespace Infrastructure.DataIOs;

public sealed class ProductDemandFileWriter(string path, string separator) : IDataWriter<ProductDemand>
{
    public void WriteData(ProductDemand productDemand)
    {
        var appendHeader = !File.Exists(path); 
        
        using var streamWriter = new StreamWriter(path, append: true);
        using var csvWriter = new CsvWriter(streamWriter,
            new CsvConfiguration(CultureInfo.InvariantCulture) {Delimiter = separator});
        csvWriter.Context.RegisterClassMap<DemandMap>();
        
        if (appendHeader)
        {
            
            csvWriter.WriteHeader<ProductDemand>();
            csvWriter.NextRecord();
        }
        
        csvWriter.WriteRecord(productDemand);
        csvWriter.NextRecord();
    }
}