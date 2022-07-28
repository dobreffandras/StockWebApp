using StockWebApp.Dtos;

namespace StockWebApp.Services
{
    public class StocksService
    {
        private readonly IDictionary<string, Stock> stocks;
        private readonly IDictionary<string, IEnumerable<StockPrice>> yearlyPrices;

        public StocksService()
        {
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

        public IEnumerable<StockPrice> GetPricesFor(string symbol)
        {
            if(stocks.TryGetValue(symbol, out var stock))
            {
                return yearlyPrices[stock.Company.Symbol];
            }

            return Array.Empty<StockPrice>();
        }

        private IEnumerable<StockPrice> GenerateYearlyPrices(double startPrice)
        {
            var priceDeltaGenerator = new RandomPriceDeltaGenerator();
            var today = DateTime.Now;
            var aYearAgo = today.AddYears(-1);
            var forces = Enumerable.Range(0, new Random().Next(1, 11)).Select(_ => priceDeltaGenerator.Generate()).ToArray();
            
            return GeneratePrices(
                startPrice,
                aYearAgo,
                today,
                priceDeltaGenerator.Generate,
                forces);
        }

        // Exposed only for testing purposes
        public IEnumerable<StockPrice> GeneratePrices(
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
