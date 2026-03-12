using HackerNews.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HackerNews.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoriesController : ControllerBase
    {
        private readonly IBestStoriesService _service;

        public StoriesController(IBestStoriesService service)
        {
            _service = service;
        }

        [HttpGet("best")]
        public async Task<IActionResult> GetBestStories([FromQuery] int n)
        {
            var result = await _service.GetBestStoriesAsync(n);
            return Ok(result);
        }

        [HttpGet("cache-only")]
        public async Task<IActionResult> GetBestStoriesFromCache([FromQuery] int n)
        {
            var result = await _service.GetBestStoriesAsync(n);
            return Ok(result);
        }
    }
}
