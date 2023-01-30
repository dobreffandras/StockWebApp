using StockWebApp.Dtos;

namespace StockWebApp.Services
{
    public class StocksService
    {
        private readonly IDictionary<string, Stock> stocks;
        private readonly IDictionary<string, IEnumerable<StockPrice>> yearlyPrices;
        private readonly IDictionary<string, IEnumerable<StockPrice>> dailyPrices;

        public StocksService(Data data)
        {
            stocks = data.Stocks.ToDictionary(x => x.Company.Symbol, x => x);

            yearlyPrices = 
                stocks.ToDictionary(
                    x => x.Key,
                    x => GenerateYearlyPrices(x.Value.Price));

            dailyPrices =
                stocks.ToDictionary(
                    x => x.Key,
                    x => GenerateDailyPrices(x.Value.Price));
        }


        public IEnumerable<BasicStock> GetStocks()
        {
            return stocks.Values.Select(
                stock => new BasicStock(
                    stock.Company,
                    stock.Price,
                    stock.Currency,
                    stock.ChangePoint,
                    stock.ChangePercent));
        }

        public Stock? GetStock(string symbol)
        {
            if (stocks.TryGetValue(symbol, out var stock))
            {
                return stock;
            }

            return null;
        }

        public IEnumerable<StockPrice> GetYearlyPricesFor(string symbol)
        {
            if(stocks.TryGetValue(symbol, out var stock))
            {
                return yearlyPrices[stock.Company.Symbol];
            }

            return Array.Empty<StockPrice>();
        }

        public IEnumerable<StockPrice> GetDailyPricesFor(string symbol)
        {
            if (stocks.TryGetValue(symbol, out var stock))
            {
                return dailyPrices[stock.Company.Symbol];
            }

            return Array.Empty<StockPrice>();
        }

        public async Task AttachLivePriceListener(
            string symbol,
            Func<StockPrice, Task> updatePrice, 
            CancellationToken ct)
        {
            if (stocks.TryGetValue(symbol, out var stock))
            {
                var nextPriceGenerator = 
                    new NextPriceGenerator(
                        stock.Price, 
                        new RandomPriceDeltaGenerator().Generate);
                while (!ct.IsCancellationRequested)
                {
                    await Task.Delay(500, ct);
                    nextPriceGenerator.Generate();
                    await updatePrice(
                        new StockPrice(DateTime.UtcNow, nextPriceGenerator.Price));
                }
            } 
            else
            {
                throw new ArgumentException("This symbol does not exist.", nameof(symbol));
            }
        }

        private IEnumerable<StockPrice> GenerateYearlyPrices(double startPrice)
        {
            var priceDeltaGenerator = new RandomPriceDeltaGenerator();
            var today = DateTime.Now;
            var aYearAgo = today.AddYears(-1);
            var forces = Enumerable.Range(0, new Random().Next(1, 11)).Select(_ => priceDeltaGenerator.Generate()).ToArray();
            
            return GeneratePrices(
                startPrice,
                DatesBetween(aYearAgo, today),
                priceDeltaGenerator.Generate,
                forces);
        }

        private IEnumerable<StockPrice> GenerateDailyPrices(double startPrice)
        {
            var priceDeltaGenerator = new RandomPriceDeltaGenerator();
            var today = DateTime.Now;
            var yesterday = today.AddDays(-1);
            var forces = Enumerable.Range(0, new Random().Next(1, 11)).Select(_ => priceDeltaGenerator.Generate()).ToArray();

            return GeneratePrices(
                startPrice,
                MinutesBetween(yesterday, today),
                priceDeltaGenerator.Generate,
                forces);
        }

        // Exposed only for testing purposes
        public IEnumerable<StockPrice> GeneratePrices(
            double startValue, 
            DateTime[] timePoints,
            Func<double> randomFeed,
            double[] forces)
        {
            var nextPriceGenerator = new NextPriceGenerator(startValue, randomFeed);
            var totalTimePoints = timePoints.Length;
            var stockPrices = new List<StockPrice>
            {
                new StockPrice(timePoints[0], startValue)
            };

            var rate =  (double)forces.Length / totalTimePoints;
            for (var timePointNum = 1; timePointNum < totalTimePoints; timePointNum++)
            {
                var forceIdx = (int)(timePointNum * rate);
                var force = forces[forceIdx];
                nextPriceGenerator.Force = force;
                nextPriceGenerator.Generate();
                stockPrices.Add(new StockPrice(timePoints[timePointNum], nextPriceGenerator.Price));
            }

            return stockPrices;
        }

        private static DateTime[] DatesBetween(DateTime startDate, DateTime endDate)
        {
            return Enumerable.Range(0, endDate.Subtract(startDate).Days)
              .Select(offset => startDate.AddDays(offset))
              .ToArray();
        }

        private static DateTime[] MinutesBetween(DateTime startDate, DateTime endDate)
        {
            return Enumerable.Range(0, (int)endDate.Subtract(startDate).TotalMinutes)
              .Select(offset => startDate.AddMinutes(offset))
              .ToArray();
        }
    }
}
