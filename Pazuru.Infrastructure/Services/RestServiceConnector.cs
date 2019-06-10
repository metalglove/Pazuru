using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pazuru.Application.Interfaces;

namespace Pazuru.Infrastructure.Services
{
    public class RestServiceConnector : IRestServiceConnector
    {
        private readonly IHttpHandler _httpHandler;
        private readonly Uri _uri;

        public RestServiceConnector(IHttpHandler httpClient)
        {
            _httpHandler = httpClient;
            _uri = new UriBuilder("http", "localhost", 8090).Uri;
        }

        private static StringContent GetHttpContent(string jsonObject)
            => new StringContent(jsonObject, Encoding.UTF8, "application/json");

        public async Task<TResponse> PostAsync<TResponse, TRequest>(string queryPath, TRequest @object)
        {
            Uri requestUri = new Uri(_uri, queryPath);
            string jsonRequest = JsonConvert.SerializeObject(@object);
            Console.WriteLine($"POST: {queryPath}");
            Console.WriteLine(jsonRequest);
            StringContent stringContent = GetHttpContent(jsonRequest);
            HttpResponseMessage response = await _httpHandler.PostAsync(requestUri, stringContent);
            response.EnsureSuccessStatusCode();
            string jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Response:");
            Console.WriteLine(jsonResponse);
            TResponse responseT = JsonConvert.DeserializeObject<TResponse>(jsonResponse);
            return responseT;
        }

        public async Task<TResponse> GetAsync<TResponse>(string queryPath)
        {
            Uri requestUri = new Uri(_uri, queryPath);
            Console.WriteLine($"GET: {queryPath}");
            HttpResponseMessage response = await _httpHandler.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            string jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Response:");
            Console.WriteLine(jsonResponse);
            TResponse responseT = JsonConvert.DeserializeObject<TResponse>(jsonResponse);
            return responseT;
        }
    }
}
