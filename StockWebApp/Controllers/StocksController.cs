using Microsoft.AspNetCore.Mvc;
using StockWebApp.Dtos;

namespace StockWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StocksController : ControllerBase
    {
        private readonly IDictionary<string, Stock> stocks 
            = new Dictionary<string, Stock>
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

        [HttpGet]
        public IEnumerable<BasicStock> GetStocks()
        {
            Thread.Sleep(500);
            return stocks.Values.Select(
                stock => new BasicStock(
                    stock.Company,
                    stock.Price,
                    stock.Currency,
                    stock.ChangePoint,
                    stock.ChangePercent));
        }

        [HttpGet("{symbol}")]
        public ActionResult<Stock> GetStock(string symbol)
        {
            Thread.Sleep(500);
            if (stocks.TryGetValue(symbol, out var stock))
            {
                return Ok(stock);
            }

            return NotFound();
        }

        [HttpGet("{symbol}/prices")]
        public ActionResult<IEnumerable<StockPrice>> GetPrices()
        {
            var random = new Random();
            var today = DateTime.Now;
            return Ok(Enumerable.Range(0, 365)
                .Select(dayNum =>
                {
                    var date = today.AddDays(-365 + dayNum);
                    return new StockPrice(date, random.Next());
                }));
        }
    }
}
