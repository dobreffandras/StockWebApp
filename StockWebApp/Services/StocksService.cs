using StockWebApp.Dtos;

namespace StockWebApp.Services
{
    public class StocksService
    {
        private readonly IDictionary<string, Stock> stocks;
        private readonly IDictionary<string, IEnumerable<StockPrice>> yearlyPrices;
        private readonly IDictionary<string, IEnumerable<StockPrice>> dailyPrices;

        public StocksService()
        {
            // TODO get this from config
            stocks = new Dictionary<string, Stock>
            {
                ["AAPL"] = new Stock(
                            new Company("AAPL", "NASDAQ", "Apple Inc."),
                            150.6,
                            "USD",
                            -1.26,
                            0.81,
                            152.95,
                            154.01,
                            2480000000,
                            new PriceRange(152.28, 155.04),
                            new PriceRange(129.04, 182.94),
                            0.92,
                            0.6),
                ["GOOG"] = new Stock(
                            new Company("GOOG", "NASDAQ", "Alphabet Inc Class C"),
                            108.36,
                            "USD",
                            -6.68,
                            5.81,
                            108.36,
                            108.88,
                            1420000000,
                            new PriceRange(107.01, 110.57),
                            new PriceRange(102.21, 152.10),
                            null,
                            null),
                ["NFLX"] = new Stock(
                            new Company("NFLX", "NASDAQ", "Netflix"),
                            220.44,
                            "USD",
                            3.44,
                            1.54,
                            218.51,
                            221.31,
                            97_173_000_000,
                            new PriceRange(216.35, 225.23),
                            new PriceRange(162.71, 700.99),
                            null,
                            null),
            };

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
                var newPrice = stock.Price;
                while (!ct.IsCancellationRequested)
                {
                    newPrice += 1.2;
                    await Task.Delay(500, ct);
                    await updatePrice(
                        new StockPrice(DateTime.UtcNow, newPrice));
                }
            }
            // TODO return bad request for missing symbol
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
