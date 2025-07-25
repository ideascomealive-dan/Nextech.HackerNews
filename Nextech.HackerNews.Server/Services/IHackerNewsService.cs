using Nextech.HackerNews.Server.Models;

namespace Nextech.HackerNews.Server.Services
{
    public interface IHackerNewsService
    {
        Task<List<StoryDto>> GetNewestStoriesAsync(int page, int pageSize, string search = "");
    }
}
