using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nextech.HackerNews.Server.Services;

namespace Nextech.HackerNews.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoriesController : ControllerBase
    {
        private readonly IHackerNewsService _service;

        public StoriesController(IHackerNewsService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string search = "")
        {
            var stories = await _service.GetNewestStoriesAsync(page, pageSize, search);
            return Ok(stories);
        }
    }

}
