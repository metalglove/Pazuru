using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Pazuru.Application.Interfaces
{
    public interface IWebSocket
    {
        WebSocketState State { get; }

        Task SendAsync(
            ArraySegment<byte> buffer,
            WebSocketMessageType messageType,
            bool endOfMessage,
            CancellationToken cancellationToken);

        Task<WebSocketReceiveResult> ReceiveAsync(
            ArraySegment<byte> buffer,
            CancellationToken cancellationToken);

        Task CloseAsync(
            WebSocketCloseStatus closeStatus,
            string statusDescription,
            CancellationToken cancellationToken);
    }
}
