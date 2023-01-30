namespace StockWebApp.Model
{
    public record BasicStock(
        Company Company,
        double StockPrice,
        string Currency,
        double ChangePoint,
        double ChangePercent);
}
