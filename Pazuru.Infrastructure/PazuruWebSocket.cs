using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Pazuru.Application.Interfaces;

namespace Pazuru.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class PazuruWebSocket : IWebSocket
    {
        private readonly WebSocket _webSocket;
        public WebSocketState State => _webSocket.State;

        public PazuruWebSocket(WebSocket webSocket)
        {
            _webSocket = webSocket;
        }

        public Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
            => _webSocket.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
        public Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
            => _webSocket.ReceiveAsync(buffer, cancellationToken);
        public Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
            => _webSocket.CloseAsync(closeStatus, statusDescription, cancellationToken);
    }
}
