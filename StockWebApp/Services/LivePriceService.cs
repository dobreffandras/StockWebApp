using StockWebApp.Model;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace StockWebApp.Services
{
    public class LivePriceService
    {
        private readonly WebSocket webSocket;
        private readonly StocksService stocksService;

        public LivePriceService(WebSocket webSocket, StocksService stocksService)
        {
            this.webSocket = webSocket;
            this.stocksService = stocksService;
        }

        public async Task SendPriceDataFor(string symbol)
        {
            var ctSource = new CancellationTokenSource();
            var closeTask = CancelOnCloseMessageReceived(ctSource);
            var senderTask = SendPriceUpdates(symbol, ctSource.Token);

            await Task.WhenAny(closeTask, senderTask);

            if (senderTask.IsFaulted)
            {
                var ex = senderTask.Exception!;
                await webSocket.CloseAsync(
                    closeStatus: WebSocketCloseStatus.NormalClosure,
                    statusDescription: ex.Message,
                    CancellationToken.None);
                throw ex.InnerException!;
            }
            else
            {
                await webSocket.CloseAsync(
                    closeStatus: WebSocketCloseStatus.NormalClosure,
                    statusDescription: "Socket state changed",
                    CancellationToken.None);
            }
        }

        private Task SendPriceUpdates(
            string symbol,
            CancellationToken cancellationToken)
            => stocksService.AttachLivePriceListener(
                symbol,
                SendPriceToWebSocket,
                cancellationToken);

        private Task SendPriceToWebSocket(StockPrice newPrice)
        {
            var message = JsonSerializer.Serialize(
                newPrice,
                new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            var bytes = Encoding.Default.GetBytes(message);
            return webSocket.SendAsync(
                new ArraySegment<byte>(bytes),
                WebSocketMessageType.Text,
                endOfMessage: true,
                CancellationToken.None);
        }

        private async Task CancelOnCloseMessageReceived(
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
