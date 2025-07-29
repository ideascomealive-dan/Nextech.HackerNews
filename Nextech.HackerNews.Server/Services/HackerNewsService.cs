using Microsoft.Extensions.Options;
using Nextech.HackerNews.Server.Configurations;
using Nextech.HackerNews.Server.Models;

namespace Nextech.HackerNews.Server.Services
{
    public class HackerNewsService : IHackerNewsService
    {
        private readonly HttpClient _httpClient;
        private readonly ICacheProvider _cache;
        private readonly string _baseUrl;

        public HackerNewsService(
            HttpClient httpClient,
            ICacheProvider cache,
            IOptions<HackerNewsApiSettings> settings)
        {
            _httpClient = httpClient;
            _cache = cache;
            _baseUrl = settings.Value.BaseUrl;
        }

        public async Task<List<StoryDto?>> GetNewestStoriesAsync(int page, int pageSize, string search = "")
        {
            var storyIds = await _cache.GetOrCreateAsync("newstories", async () => await _httpClient.GetFromJsonAsync<List<int>>($"{_baseUrl}newstories.json") ?? new List<int>());

            var storiesToFetch = storyIds!.Skip((page - 1) * pageSize).Take(pageSize);
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
