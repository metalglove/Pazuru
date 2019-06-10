using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Pazuru.Application.Interfaces;
using Pazuru.Infrastructure;

namespace Pazuru.Presentation.Web.BackEnd
{
    // NOTE: Excluded because PazuruWebSocket is excluded and this class only gets
    // called by the middleware pipeline and invokes a websocket request. 
    // This is covered in the tests by a received mock of the IWebSocket interface.
    [ExcludeFromCodeCoverage]
    public class WebSocketManagerMiddleware
    {
        private WebSocketHandler WebSocketHandler { get; }

        public WebSocketManagerMiddleware(RequestDelegate next,
            WebSocketHandler webSocketHandler)
        {
            _ = next; // NOTE: Ignore from pipeline
            WebSocketHandler = webSocketHandler;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
                return;

            PazuruWebSocket socket = new PazuruWebSocket(await context.WebSockets.AcceptWebSocketAsync());
            await WebSocketHandler.OnConnected(socket);

            await Receive(socket, async (result, buffer) =>
            {
                switch (result.MessageType)
                {
                    case WebSocketMessageType.Text:
                        await WebSocketHandler.ReceiveAsync(socket, buffer);
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
        }

        private static async Task Receive(IWebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            byte[] buffer = new byte[1024 * 4];

            while (socket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer), cancellationToken: CancellationToken.None);
                handleMessage(result, buffer);
                buffer = new byte[1024 * 4];
            }
        }
    }
}
