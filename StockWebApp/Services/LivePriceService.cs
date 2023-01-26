using System.Net.WebSockets;
using System.Text;

namespace StockWebApp.Services
{
    public class LivePriceService
    {
        private readonly StocksService stocksService;

        public LivePriceService(StocksService stocksService)
        {
            this.stocksService = stocksService;
        }

        public async Task SendPriceDataFor(WebSocket webSocket)
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
