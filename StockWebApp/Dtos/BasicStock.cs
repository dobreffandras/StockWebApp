namespace StockWebApp.Dtos
{
    public record BasicStock(
        Company Company,
        double StockPrice,
        string Currency,
        double ChangePoint,
        double Changepercent);
}
