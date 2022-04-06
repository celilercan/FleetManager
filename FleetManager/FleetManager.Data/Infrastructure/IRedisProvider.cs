using System.Threading.Tasks;

namespace FleetManager.Data.Infrastructure
{
    public interface IRedisProvider
    {
        Task<T> GetAsync<T>(string key);

        Task SetAsync<T>(string key, T data);

        Task<bool> IsExistAsync(string key);

        Task RemoveAsync(string key);
    }
}
