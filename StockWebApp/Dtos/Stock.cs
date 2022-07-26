namespace StockWebApp.Dtos
{
    public record Stock(
        Company Company,
        double Price,
        string Currency,
        double ChangePoint,
        double ChangePercent,
        double PreviousClose,
        double Open,
        double MarketCap,
        PriceRange DailyRange,
        PriceRange YearlyRange,
        double? Dividend,
        double? DividendYield);

    public record PriceRange(double Low, double High);
}
