using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using Nextech.HackerNews.Server.Configurations;
using Nextech.HackerNews.Server.Models;
using Nextech.HackerNews.Server.Services;
using System.Net.Http;

namespace Nextech.HackerNews.Test
{
    public class HackerNewsServiceTests
    {
        private readonly Mock<HttpMessageHandler> _mockHttpHandler;
        private readonly HttpClient _httpClient;
        private readonly MemoryCacheProvider _memoryCache;
        private readonly HackerNewsService _service;

        public HackerNewsServiceTests()
        {
            _mockHttpHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpClient = new HttpClient(_mockHttpHandler.Object)
            {
                BaseAddress = new Uri("https://hacker-news.firebaseio.com/v0/")
            };

            _memoryCache = new MemoryCacheProvider( new MemoryCache(new MemoryCacheOptions()));

            var settings = Options.Create(new HackerNewsApiSettings
            {
                BaseUrl = "https://hacker-news.firebaseio.com/v0/"
            });

            _service = new HackerNewsService(_httpClient, _memoryCache, settings);
        }

        [Fact]
        public async Task GetNewestStoriesAsync_ReturnsStories_WithValidTitleAndUrl()
        {
            // Arrange
            var storyIds = new List<int> { 101, 102 };

            var storyData = new List<StoryDto>
            {
                new StoryDto { Title = "Story 1", Url = "http://example.com/1" },
                new StoryDto { Title = "Story 2", Url = null }
            };

            _mockHttpHandler.ReturnsJson($"{_httpClient.BaseAddress}newstories.json", storyIds);
            _mockHttpHandler.ReturnsJson($"{_httpClient.BaseAddress}item/101.json", storyData[0]);
            _mockHttpHandler.ReturnsJson($"{_httpClient.BaseAddress}item/102.json", storyData[1]);

            // Act
            var result = await _service.GetNewestStoriesAsync(page: 1, pageSize: 10);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, s => s.Title == "Story 1");
            Assert.Contains(result, s => s.Title == "Story 2");
        }

        [Fact]
        public async Task GetNewestStoriesAsync_CachesStoryIds()
        {
            // Arrange
            var storyIds = new List<int> { 101 };

            _mockHttpHandler.ReturnsJson($"{_httpClient.BaseAddress}newstories.json", storyIds);
            _mockHttpHandler.ReturnsJson($"{_httpClient.BaseAddress}item/101.json", new StoryDto { Title = "Cached Story", Url = "http://cached.com" });


            // Act
            var result1 = await _service.GetNewestStoriesAsync(1, 1);
            var result2 = await _service.GetNewestStoriesAsync(1, 1); // This should hit the cache

            // Assert
            Assert.Single(result1);
            Assert.Single(result2);
            _mockHttpHandler.VerifyRequest($"{_httpClient.BaseAddress}newstories.json", Times.Once());
        }
    }
}