using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Pazuru.Application.Interfaces;

namespace Pazuru.Presentation.Web.BackEnd
{
    public abstract class WebSocketHandler
    {
        protected WebSocketConnectionManager WebSocketConnectionManager { get; set; }

        protected WebSocketHandler(WebSocketConnectionManager webSocketConnectionManager)
        {
            WebSocketConnectionManager = webSocketConnectionManager;
        }

        public virtual Task OnConnected(IWebSocket socket)
        {
            WebSocketConnectionManager.AddSocket(socket);
            return Task.CompletedTask;
        }

        public virtual async Task OnDisconnected(IWebSocket socket)
        {
            await WebSocketConnectionManager.RemoveSocket(WebSocketConnectionManager.GetId(socket));
        }

        public async Task SendMessageAsync(IWebSocket socket, string message)
        {
            if (socket.State != WebSocketState.Open)
                return;

            await socket.SendAsync(buffer: new ArraySegment<byte>(array: Encoding.Default.GetBytes(message),
                    offset: 0,
                    count: message.Length),
                messageType: WebSocketMessageType.Text,
                endOfMessage: true,
                cancellationToken: CancellationToken.None);
        }

        public abstract Task ReceiveAsync(IWebSocket socket, byte[] buffer);
    }
}
