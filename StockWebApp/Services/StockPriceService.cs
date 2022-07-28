using StockWebApp.Dtos;

namespace StockWebApp.Services
{
    public class StockPriceService
    {
        public IEnumerable<StockPrice> GenerateDailyPrices(
            double startValue, 
            DateTime startDate, 
            DateTime endDate, 
            Func<double> randomFeed,
            double[] forces)
        {
            var nextPriceGenerator = new NextPriceGenerator(startValue, randomFeed);
            var totalDays = (endDate - startDate).TotalDays;
            var stockPrices = new List<StockPrice>
            {
                new StockPrice(startDate, startValue)
            };

            var rate = forces.Length / totalDays;
            for (var dayNum = 1; dayNum < totalDays; dayNum++)
            {
                var forceIdx = (int)(dayNum * rate);
                var force = forces[forceIdx];
                nextPriceGenerator.Force = force;
                nextPriceGenerator.Generate();
                stockPrices.Add(new StockPrice(startDate.AddDays(dayNum), nextPriceGenerator.Price));
            }

            return stockPrices;
        }
    }
}
