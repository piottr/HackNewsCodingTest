using HackerNews.API.Dto;
using HackerNews.API.Services.Interfaces;
using HackerNews.Contracts.Cache;
using HackerNews.Application.Interfaces;
using HackerNews.Application.Dto;

namespace HackerNews.API.Services;

public class BestStoriesService: IBestStoriesService
{
    private readonly IHackerNewsService _client;
    private readonly ICacheService _cache; 

    public BestStoriesService(IHackerNewsService client, ICacheService cache)
    {
        _client = client;
        _cache = cache;
    }

    public async Task<List<StoryDto>> GetBestStoriesAsync(int n)
    {
        var cacheStories = await GetCacheStories(n);
        if (cacheStories != null)
        {
            return [..cacheStories.Select(MapToStoryDto)];
        }

        var ids = await _client.GetBestStoryIdsAsync();
        var items = await FetchItemsWithLimitAsync(ids.Take(n), 10);

        var stories = items
            .Where(x => x != null)
            .Select(MapToStoryDto)
            .OrderByDescending(x => x.Score)
            .ToList();

        await SetCacheStories(stories, n);

        return stories ?? new List<StoryDto>();
    }

    private async Task SetCacheStories(List<StoryDto> stories, int n)
    {
        var cacheKey = string.Format(CacheKeys.BestStoriesPrefix, n);

        await _cache.SetAsync<List<StoryDto>>(
            cacheKey,
            stories,
            TimeSpan.FromMinutes(5)
            );
    }

    private async Task<List<HackerNewsItemDto>?> GetCacheStories(int n)
    {
        var cacheKey = string.Format(CacheKeys.BestStoriesPrefix, n);
        var cached = await _cache.GetAsync<List<HackerNewsItemDto>>(cacheKey);

        return cached;
    }

    private async Task<List<HackerNewsItemDto>> FetchItemsWithLimitAsync(IEnumerable<int> ids, int maxConcurrency)
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

    private StoryDto MapToStoryDto(HackerNewsItemDto item)
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
