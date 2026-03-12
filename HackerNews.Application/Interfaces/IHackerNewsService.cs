using HackerNews.Application.Dto;

namespace HackerNews.Application.Interfaces;

public interface IHackerNewsService
{
    Task<List<int>> GetBestStoryIdsAsync();

    Task<HackerNewsItemDto> GetItemAsync(int n);
}
