using Microsoft.AspNetCore.Mvc;
using StockWebApp.Dtos;

namespace StockWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StocksController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<BasicStock> GetWithBaseInfo()
        {
            return new[]
            {
                new BasicStock(
                    new Company("AAPL", "NASDAQ", "Apple Inc."),
                    150.6,
                    "USD",
                    -1.26,
                    0.81),
                new BasicStock(
                    new Company("GOOG", "NASDAQ", "Alphabet Inc Class C"),
                    108.36,
                    "USD",
                    -6.68,
                    5.81),
                new BasicStock(
                    new Company("NFLX", "NASDAQ", "Netflix"),
                    220.44,
                    "USD",
                    -3.44,
                    1.54)
            };
        }
    }
}
