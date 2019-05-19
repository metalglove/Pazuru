﻿using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Pazuru.Presentation.Web.BackEnd
{
    public class WebSocketConnectionManager
    {
        private readonly ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

        public WebSocket GetSocketById(string id)
        {
            return _sockets.FirstOrDefault(p => p.Key == id).Value;
        }

        public ConcurrentDictionary<string, WebSocket> GetAll()
        {
            return _sockets;
        }

        public string GetId(WebSocket socket)
        {
            return _sockets.FirstOrDefault(p => p.Value == socket).Key;
        }
        public void AddSocket(WebSocket socket)
        {
            _sockets.TryAdd(CreateConnectionId(), socket);
        }

        public async Task RemoveSocket(string id)
        {
            _sockets.TryRemove(id, out WebSocket socket);

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
