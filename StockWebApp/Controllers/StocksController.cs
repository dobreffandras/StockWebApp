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
            stocksService = new StocksService();
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

        [HttpGet("{symbol}/prices")]
        public ActionResult<IEnumerable<StockPrice>> GetPrices(string symbol)
        {
            return Ok(stocksService.GetPricesFor(symbol));
        }
    }
}
