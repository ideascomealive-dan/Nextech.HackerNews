using Microsoft.Extensions.Caching.Memory;

namespace Nextech.HackerNews.Server.Services
{
    public interface ICacheProvider
    {
        Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T>> fetch, TimeSpan? expiration = null);
    }

    public class MemoryCacheProvider : ICacheProvider
    {
        private readonly IMemoryCache _cache;

        public MemoryCacheProvider(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T>> fetch, TimeSpan? expiration = null)
        {
            if (_cache.TryGetValue(key, out T? value))
                return value;

            value = await fetch();

            if (value != null)
            {
                _cache.Set(key, value, expiration ?? TimeSpan.FromMinutes(5));
            }

            return value;
        }
    }

}
