using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Pazuru.Presentation.Web.BackEnd
{
    public class WebSocketManagerMiddleware
    {
        private readonly RequestDelegate _next;
        private WebSocketHandler WebSocketHandler { get; set; }

        public WebSocketManagerMiddleware(RequestDelegate next,
            WebSocketHandler webSocketHandler)
        {
            _next = next;
            WebSocketHandler = webSocketHandler;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
                return;

            WebSocket socket = await context.WebSockets.AcceptWebSocketAsync();
            await WebSocketHandler.OnConnected(socket);

            await Receive(socket, async (result, buffer) =>
            {
                switch (result.MessageType)
                {
                    case WebSocketMessageType.Text:
                        await WebSocketHandler.ReceiveAsync(socket, result, buffer);
                        return;
                    case WebSocketMessageType.Close:
                        await WebSocketHandler.OnDisconnected(socket);
                        return;
                    case WebSocketMessageType.Binary:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });

            //TODO - investigate the Kestrel exception thrown when this is the last middleware
            //await _next.Invoke(context);
        }

        private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            byte[] buffer = new byte[1024 * 4];

            while (socket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
                    cancellationToken: CancellationToken.None);

                handleMessage(result, buffer);
            }
        }
    }
}
