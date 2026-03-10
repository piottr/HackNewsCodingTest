using HackerNews.API.Client.Interfaces;
using HackerNews.API.Common.Const;
using HackerNews.API.Dto;
using HackerNews.API.Models;
using HackerNews.API.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace HackerNews.API.Services;

public class BestStoriesService: IBestStoriesService
{
    private readonly IHackerNewsService _client;
    private readonly IDistributedCache _cache; 

    public BestStoriesService(IHackerNewsService client, IDistributedCache cache)
    {
        _client = client;
        _cache = cache;
    }

    public async Task<List<StoryDto>> GetBestStoriesAsync(int n)
    {
        var cacheStories = await GetCacheStories(n);
        if (cacheStories != null)
        {
            return cacheStories;
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

        await _cache.SetStringAsync(
            cacheKey,
            JsonSerializer.Serialize(stories),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
    }

    private async Task<List<StoryDto>?> GetCacheStories(int n)
    {
        var cacheKey = string.Format(CacheKeys.BestStoriesPrefix, n);
        var cached = await _cache.GetStringAsync(cacheKey);

        if (cached != null)
            return JsonSerializer.Deserialize<List<StoryDto>>(cached);

        return null;
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
