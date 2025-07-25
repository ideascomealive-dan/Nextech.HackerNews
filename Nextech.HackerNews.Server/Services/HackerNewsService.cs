using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Nextech.HackerNews.Server.Configurations;
using Nextech.HackerNews.Server.Models;

namespace Nextech.HackerNews.Server.Services
{
    public class HackerNewsService : IHackerNewsService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly string _baseUrl;

        public HackerNewsService(
            HttpClient httpClient,
            IMemoryCache cache,
            IOptions<HackerNewsApiSettings> settings)
        {
            _httpClient = httpClient;
            _cache = cache;
            _baseUrl = settings.Value.BaseUrl;
        }

        public async Task<List<StoryDto>> GetNewestStoriesAsync(int page, int pageSize, string search = "")
        {
            if (!_cache.TryGetValue("newstories", out List<int> storyIds))
            {
                var response = await _httpClient.GetFromJsonAsync<List<int>>($"{_baseUrl}newstories.json");
                storyIds = response ?? new List<int>();
                _cache.Set("newstories", storyIds, TimeSpan.FromMinutes(5));
            }

            var storiesToFetch = storyIds.Skip((page - 1) * pageSize).Take(pageSize);
            var tasks = storiesToFetch.Select(id => _httpClient.GetFromJsonAsync<StoryDto>($"{_baseUrl}item/{id}.json"));
            var stories = (await Task.WhenAll(tasks)).Where(s => s != null).ToList();

            if (!string.IsNullOrEmpty(search))
            {
                stories = stories
                    .Where(s => s.Title?.Contains(search, StringComparison.OrdinalIgnoreCase) == true)
                    .ToList();
            }

            return stories;
        }
    }
}
