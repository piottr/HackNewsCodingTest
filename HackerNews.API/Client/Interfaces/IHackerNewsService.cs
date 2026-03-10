using HackerNews.API.Dto;
using HackerNews.API.Models;

namespace HackerNews.API.Client.Interfaces;

public interface IHackerNewsService
{
    Task<List<int>> GetBestStoryIdsAsync();

    Task<HackerNewsItem> GetItemAsync(int n);
}
