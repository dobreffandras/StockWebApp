using Microsoft.AspNetCore.Mvc;
using StockWebApp.Dtos;
using StockWebApp.Services;

namespace StockWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StocksController : ControllerBase
    {
        public StocksController(StocksService stocksService)
        {
            this.stocksService = stocksService;
        }

        private StocksService stocksService;

        [HttpGet]
        public IEnumerable<BasicStock> GetStocks()
        {
            Thread.Sleep(500);
            return stocksService.GetStocks();
        }

        [HttpGet("{symbol}")]
        public ActionResult<Stock> GetStock(string symbol)
        {
            Thread.Sleep(500);
            if(stocksService.GetStock(symbol) is { } stock)
            {
                return Ok(stock);
            }

            return NotFound();
        }

        /// <summary>
        /// Gets the prices of the specified stock for a yearly or daily range.
        /// Yearly prices will be sampled on daily bases. Daily prices will be sampled on minute basis.
        /// </summary>
        /// <param name="symbol">The symbol of the stock</param>
        /// <param name="interval">Either "day" for daily sampling or anything else for yearly sampling</param>
        [HttpGet("{symbol}/prices")]
        public ActionResult<IEnumerable<StockPrice>> GetPrices(
            string symbol,
            [FromQuery] string interval)
        {
            if(interval == "day")
            {
                return Ok(stocksService.GetDailyPricesFor(symbol));
            } else
            {
                return Ok(stocksService.GetYearlyPricesFor(symbol));
            }
            
        }
    }
}
