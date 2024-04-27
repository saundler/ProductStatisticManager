namespace Domain.Models;

public class ProductStat
{
    public long Id { get; set; } 
    public long Prediction { get; set; } 
    public long Stock { get; set; }

    public ProductStat() { }

    public ProductStat(long id, long prediction, long stock)
    {
        Id = id;
        Prediction = prediction;
        Stock = stock;
    }

    public override string ToString()
    {
        return "ProductStat { " + $"Id = {Id}, Prediction = {Prediction}, Stock = {Stock}" + " }";
    }
}