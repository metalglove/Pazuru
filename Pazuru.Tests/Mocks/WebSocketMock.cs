using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pazuru.Application.Interfaces;
using Pazuru.Presentation.Web.BackEnd.Utilities;

namespace Pazuru.Tests.Mocks
{
    public class WebSocketMock : IWebSocket
    {
        private readonly List<(PreMessage, ArraySegment<byte>)> _sentMessages = new List<(PreMessage, ArraySegment<byte>)>();
        private readonly List<(PreMessage, ArraySegment<byte>)> _receivedMessages = new List<(PreMessage, ArraySegment<byte>)>();

        public WebSocketState State { get; private set; } = WebSocketState.Open;
        public IReadOnlyList<(PreMessage preMessage, ArraySegment<byte> buffer)> SentMessages => _sentMessages;
        public IReadOnlyList<(PreMessage preMessage, ArraySegment<byte> buffer)> ReceivedMessages => _receivedMessages;

        public Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
        {
            _sentMessages.Add(GetPreMessageValueTuple(buffer));
            return Task.CompletedTask;
        }

        public Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
        {
            _receivedMessages.Add(GetPreMessageValueTuple(buffer));
            WebSocketReceiveResult webSocketReceiveResult = new WebSocketReceiveResult(buffer.Count, WebSocketMessageType.Text, true);
            return Task.FromResult(webSocketReceiveResult);
        }

        public Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
        {
            State = WebSocketState.Closed;
            return Task.CompletedTask;
        }

        private static (PreMessage, ArraySegment<byte>) GetPreMessageValueTuple(ArraySegment<byte> buffer)
        {
            string json = Encoding.Default.GetString(buffer);
            PreMessage preMessage = JsonConvert.DeserializeObject<PreMessage>(json);
            return (preMessage, buffer);
        }
    }
}
