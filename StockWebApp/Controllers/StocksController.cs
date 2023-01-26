using Microsoft.AspNetCore.Mvc;
using StockWebApp.Dtos;
using StockWebApp.Services;
using System.Net.WebSockets;
using System.Text;

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

        [HttpGet("/ws")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await Echo(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        private async Task Echo(WebSocket webSocket)
        {
            var ctSource = new CancellationTokenSource();
            var closeTask = CancelOnCloseMessageReceived(webSocket, ctSource);
            var senderTask = SendPriceUpdates(webSocket, ctSource.Token);

            await Task.WhenAny(closeTask, senderTask);
            await webSocket.CloseAsync(
                closeStatus: WebSocketCloseStatus.NormalClosure,
                statusDescription: "Socket state changed",
                CancellationToken.None);
        }

        private Task SendPriceUpdates(
            WebSocket webSocket,
            CancellationToken cancellationToken) 
            => stocksService.AttachLivePriceListener(
                newPrice =>
                {
                    var bytes = Encoding.Default.GetBytes(newPrice.ToString());
                    return webSocket.SendAsync(
                        new ArraySegment<byte>(bytes),
                        WebSocketMessageType.Text,
                        endOfMessage: true,
                        CancellationToken.None);
                },
                cancellationToken);

        private static async Task CancelOnCloseMessageReceived(
            WebSocket webSocket, 
            CancellationTokenSource ctSource)
        {
            while (true)
            {
                var segment = new ArraySegment<byte>(new byte[2 * 1024]);
                var msg = await webSocket.ReceiveAsync(segment, ctSource.Token);
                if (msg.CloseStatus.HasValue)
                {
                    ctSource.Cancel();
                }
                // ignore other messages
            }
        }
    }
}
