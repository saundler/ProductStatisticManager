using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Domain.Models;
using Infrastructure.DataIOs.Models;

namespace Infrastructure.DataIOs;

public sealed class ProductStatFileReader(string path, string separator) : IDataReader<ProductStat>
{
    public IEnumerable<ProductStat> ReadData()
    {
        using var streamReader = new StreamReader(path);
        using var csvReader = new CsvReader(streamReader,
            new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = true, Delimiter = separator });
        csvReader.Context.RegisterClassMap<ProductMap>();
        while (csvReader.Read())
        {
            yield return csvReader.GetRecord<ProductStat>();
        }
    }
}