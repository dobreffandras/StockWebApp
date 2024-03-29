﻿using Microsoft.AspNetCore.Mvc;
using StockWebApp.Model;
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
            Thread.Sleep(500); // simulate latency
            return stocksService.GetStocks();
        }

        [HttpGet("{symbol}")]
        public ActionResult<Stock> GetStock(string symbol)
        {
            Thread.Sleep(500); // simulate latency
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

        [HttpGet("{symbol}/prices/live")]
        public async Task<IActionResult> Get(string symbol)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                try
                {
                    using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                    var livePriceService = new LivePriceService(webSocket, stocksService);
                    await livePriceService.SendPriceDataFor(symbol);
                    return Ok();
                } 
                catch (ArgumentException ex) 
                { 
                    return BadRequest(ex.Message);
                } 
                catch
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            else
            {
                return BadRequest("Not a webocket request");
            }
        }
    }
}
