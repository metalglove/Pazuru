using System.Threading.Tasks;

namespace Pazuru.Application.Interfaces
{
    public interface IRestServiceConnector
    {
        Task<TResponse> PostAsync<TResponse, TRequest>(string queryPath, TRequest @object);
        Task<TResponse> GetAsync<TResponse>(string queryPath);
    }
}
