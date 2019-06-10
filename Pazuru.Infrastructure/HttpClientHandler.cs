using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;
using Pazuru.Application.Interfaces;

namespace Pazuru.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class HttpClientHandler : IHttpHandler
    {
        private readonly HttpClient _client = new HttpClient();

        public Task<HttpResponseMessage> GetAsync(Uri url)
        {
            return _client.GetAsync(url);
        }

        public Task<HttpResponseMessage> PostAsync(Uri url, HttpContent content)
        {
            return _client.PostAsync(url, content);
        }
    }
}
