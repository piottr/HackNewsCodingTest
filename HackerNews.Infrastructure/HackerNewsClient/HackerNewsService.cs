using HackerNews.Infrastructure.Config;
using HackerNews.Application.Interfaces;
using HackerNews.Application.Dto;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace HackerNews.Infrastructure.HackerNewsClient;

public class HackerNewsService : IHackerNewsService
{
    private readonly HttpClient _httpClient;
    private readonly HackerNewsOptions _options;

    public HackerNewsService(HttpClient httpClient, IOptions<HackerNewsOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    /// <summary>
    /// Retrieves a list of IDs representing the best stories from the remote source.
    /// </summary>
    /// <remarks>If the current second is divisible by 3 and more than one ID is retrieved, the order of the
    /// IDs in the list is randomized. This may affect the order in which stories are presented to the caller.</remarks>
    public async Task<List<int>> GetBestStoryIdsAsync()
    {
        var ids = await _httpClient.GetFromJsonAsync<List<int>>(_options.BestStoriesUrl);
        var list = ids ?? new List<int>();

        if (DateTime.UtcNow.Second % 3 == 0 && list.Count > 1)
        {
            var rnd = new Random();
            int i = rnd.Next(list.Count);
            int j = rnd.Next(list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }

        return list;
    }

    /// <summary>
    /// Asynchronously retrieves a Hacker News item by its unique identifier.
    /// </summary>
    /// <remarks>The method may append a random version number to the item's title</remarks>
    /// <param name="id">The unique identifier of the Hacker News item.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the requested Hacker News item.</returns>    
    public async Task<HackerNewsItemDto> GetItemAsync(int id)
    {
        var item = await _httpClient.GetFromJsonAsync<HackerNewsItemDto>(
            string.Format(_options.ItemUrlTemplate, id));

        if (item == null)
            throw new InvalidOperationException($"HackerNews item {id} returned null");

        var rnd = new Random();

        if (rnd.Next(0, 10) == 0)
        {
            item.Title = item.Title + $" [v{rnd.Next(1, 100)}]";
        }

        return item;
    }
}