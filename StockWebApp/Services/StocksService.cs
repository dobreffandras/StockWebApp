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
                            Company: new Company("AAPL", "NASDAQ", "Apple Inc."),
                            Price: 150.6,
                            Currency: "USD",
                            ChangePoint: -1.26,
                            ChangePercent: 0.81,
                            PreviousClose: 152.95,
                            Open: 154.01,
                            MarketCap: 2480000000,
                            DailyRange: new PriceRange(152.28, 155.04),
                            YearlyRange: new PriceRange(129.04, 182.94),
                            Dividend: 0.92,
                            DividendYield: 0.6),
                ["GOOG"] = new Stock(
                            Company: new Company("GOOG", "NASDAQ", "Alphabet Inc Class C"),
                            Price: 108.36,
                            Currency: "USD",
                            ChangePoint: -6.68,
                            ChangePercent: 5.81,
                            PreviousClose: 108.36,
                            Open: 108.88,
                            MarketCap: 1420000000,
                            DailyRange: new PriceRange(107.01, 110.57),
                            YearlyRange: new PriceRange(102.21, 152.10),
                            Dividend: null,
                            DividendYield: null),
                ["NFLX"] = new Stock(
                            Company: new Company("NFLX", "NASDAQ", "Netflix"),
                            Price: 220.44,
                            Currency: "USD",
                            ChangePoint: 3.44,
                            ChangePercent: 1.54,
                            PreviousClose: 218.51,
                            Open: 221.31,
                            MarketCap: 97_173_000_000,
                            DailyRange: new PriceRange(216.35, 225.23),
                            YearlyRange: new PriceRange(162.71, 700.99),
                            Dividend: null,
                            DividendYield: null),
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
