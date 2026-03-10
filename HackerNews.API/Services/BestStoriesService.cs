using HackerNews.API.Client.Interfaces;
using HackerNews.API.Dto;
using HackerNews.API.Models;
using HackerNews.API.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNews.API.Services;

public class BestStoriesService: IBestStoriesService
{
    private readonly IHackerNewsService _client;
    private readonly IMemoryCache _cache;

    public BestStoriesService(IHackerNewsService client, IMemoryCache cache)
    {
        _client = client;
        _cache = cache;
    }

    public async Task<List<StoryDto>> GetBestStoriesAsync(int n)
    {
        var cacheKey = $"BestStories_{n}";

        if (!_cache.TryGetValue(cacheKey, out List<StoryDto>? cachedStories))
        {
            var ids = await _client.GetBestStoryIdsAsync();
            var items = await FetchItemsWithLimitAsync(ids.Take(n), 10);

            cachedStories = items
                .Where(x => x != null)
                .Select(MapToStoryDto)
                .OrderByDescending(x => x.Score)
                .ToList();

            _cache.Set(cacheKey, cachedStories, TimeSpan.FromMinutes(5));
        }

        return cachedStories ?? new List<StoryDto>();
    }

    private async Task<List<HackerNewsItem>> FetchItemsWithLimitAsync(IEnumerable<int> ids, int maxConcurrency)
    {
        var semaphore = new SemaphoreSlim(maxConcurrency);
        var tasks = ids.Select(async id =>
        {
            await semaphore.WaitAsync();
            try
            {
                return await _client.GetItemAsync(id);
            }
            finally
            {
                semaphore.Release();
            }
        });

        return (await Task.WhenAll(tasks)).ToList();
    }

    private StoryDto MapToStoryDto(HackerNewsItem item)
    {
        return new StoryDto
        {
            Title = item.Title,
            Uri = item.Url,
            PostedBy = item.By,
            Time = DateTimeOffset.FromUnixTimeSeconds(item.Time ?? 0),
            Score = item.Score ?? 0,
            CommentCount = item.Descendants ?? 0
        };
    } 
}
