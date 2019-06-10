using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Pazuru.Application.Interfaces;

namespace Pazuru.Presentation.Web.BackEnd
{
    public class WebSocketConnectionManager
    {
        private readonly ConcurrentDictionary<string, IWebSocket> _webSockets = new ConcurrentDictionary<string, IWebSocket>();

        public ConcurrentDictionary<string, IWebSocket> GetAll()
        {
            return _webSockets;
        }

        public string GetId(IWebSocket socket)
        {
            return _webSockets.FirstOrDefault(p => p.Value == socket).Key;
        }
        public void AddSocket(IWebSocket socket)
        {
            _webSockets.TryAdd(CreateConnectionId(), socket);
        }
        public async Task RemoveSocket(string id)
        {
            _webSockets.TryRemove(id, out IWebSocket socket);

            await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                statusDescription: "Closed by the WebSocketManager",
                cancellationToken: CancellationToken.None);
        }
        private static string CreateConnectionId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
