using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pazuru.Application.Interfaces
{
    public interface IHttpHandler
    {
        Task<HttpResponseMessage> GetAsync(Uri url);
        Task<HttpResponseMessage> PostAsync(Uri url, HttpContent content);
    }
}
