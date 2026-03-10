using HackerNews.API.Client.Interfaces;
using HackerNews.API.Dto;
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

        if (!_cache.TryGetValue(cacheKey, out List<StoryDto>? stories))
        {
            var ids = await _client.GetBestStoryIdsAsync();

            var semaphore = new SemaphoreSlim(10);
            var tasks = ids.Take(n).Select(async id =>
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

            var items = await Task.WhenAll(tasks);

            stories = items
                .Where(x => x != null)
                .Select(x => new StoryDto
                {
                    Title = x!.Title,
                    Uri = x!.Url,
                    PostedBy = x!.By,
                    Time = DateTimeOffset.FromUnixTimeSeconds(x.Time ?? 0),
                    Score = x.Score ?? 0,
                    CommentCount = x.Descendants ?? 0
                })
                .OrderByDescending(x => x.Score)
                .ToList();

            _cache.Set(cacheKey, stories, TimeSpan.FromMinutes(5));
        }

        return (stories ?? Enumerable.Empty<StoryDto>()).ToList();
    }
}
