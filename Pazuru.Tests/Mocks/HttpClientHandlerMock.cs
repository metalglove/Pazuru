using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Pazuru.Application.Interfaces;

namespace Pazuru.Tests.Mocks
{
    public class HttpClientHandlerMock : IHttpHandler
    {
        private readonly List<HttpResponseMessage> _responseMessages = new List<HttpResponseMessage>();

        public void AddResponse(string response)
        {
            _responseMessages.Add(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(response)
            });
        }

        public Task<HttpResponseMessage> GetAsync(Uri url)
        {
            return Task.FromResult(GetResponse());
        }

        public Task<HttpResponseMessage> PostAsync(Uri url, HttpContent content)
        {
            return Task.FromResult(GetResponse());
        }

        private HttpResponseMessage GetResponse()
        {
            HttpResponseMessage message = _responseMessages.First();
            _responseMessages.Remove(message);
            return message;
        }
    }
}
