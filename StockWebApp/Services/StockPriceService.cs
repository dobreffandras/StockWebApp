using StockWebApp.Dtos;

namespace StockWebApp.Services
{
    public class StockPriceService
    {
        public IEnumerable<StockPrice> GenerateDailyPrices(
            double startValue, 
            DateTime startDate, 
            DateTime endDate, 
            Func<double> randomFeed)
        {
            var totalDays = (endDate - startDate).TotalDays;
            var stockPrices = new List<StockPrice>
            {
                new StockPrice(startDate, startValue)
            };

            var previousValue = startValue;
            for(var dayNum = 1; dayNum < totalDays; dayNum++)
            {
                var currentValue = previousValue + randomFeed();
                stockPrices.Add(new StockPrice(startDate.AddDays(dayNum), currentValue));
                previousValue = currentValue;
            }

            return stockPrices;
        }
    }
}
