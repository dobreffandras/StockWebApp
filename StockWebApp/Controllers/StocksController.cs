using Microsoft.AspNetCore.Mvc;
using StockWebApp.Dtos;
using StockWebApp.Services;

namespace StockWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StocksController : ControllerBase
    {
        public StocksController()
        {
            stocksGenerator = new StockPriceService();
        }

        private StockPriceService stocksGenerator;

        [HttpGet]
        public IEnumerable<BasicStock> GetStocks()
        {
            Thread.Sleep(500);
            return stocksGenerator.GetStocks();
        }

        [HttpGet("{symbol}")]
        public ActionResult<Stock> GetStock(string symbol)
        {
            Thread.Sleep(500);
            if(stocksGenerator.GetStock(symbol) is { } stock)
            {
                return Ok(stock);
            }

            return NotFound();
        }

        [HttpGet("{symbol}/prices")]
        public ActionResult<IEnumerable<StockPrice>> GetPrices(string symbol)
        {
            return Ok(stocksGenerator.GetPricesFor(symbol));
        }
    }
}
