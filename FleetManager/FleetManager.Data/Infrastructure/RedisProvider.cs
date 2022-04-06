using FleetManager.Data.Constants;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace FleetManager.Data.Infrastructure
{
    public class RedisProvider : IRedisProvider
    {
        private readonly IDistributedCache _distributedCache;

        public RedisProvider(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var doc = await _distributedCache.GetStringAsync(key);
            return !string.IsNullOrEmpty(doc) ? JsonConvert.DeserializeObject<T>(doc) : default(T);
        }

        public async Task<bool> IsExistAsync(string key)
        {
            var doc = await _distributedCache.GetStringAsync(key);
            return !string.IsNullOrEmpty(doc);
        }

        public async Task RemoveAsync(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }

        public async Task SetAsync<T>(string key, T data)
        {
            await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(data), new DistributedCacheEntryOptions { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(Constant.Common.CacheMinute) });
        }
    }
}
