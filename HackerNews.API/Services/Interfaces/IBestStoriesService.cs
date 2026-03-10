using HackerNews.API.Dto;

namespace HackerNews.API.Services.Interfaces;

public interface IBestStoriesService
{
    Task<List<StoryDto>> GetBestStoriesAsync(int n);
}