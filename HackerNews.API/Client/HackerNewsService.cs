using HackerNews.API.Client.Interfaces;
using HackerNews.API.Common.Config;
using HackerNews.API.Dto;
using HackerNews.API.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace HackerNews.API.Client
{
    public class HackerNewsService : IHackerNewsService
    {
        private readonly HttpClient _httpClient;
        private readonly HackerNewsOptions _options;

        public HackerNewsService(HttpClient httpClient, IOptions<HackerNewsOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<List<int>> GetBestStoryIdsAsync()
        {
            var ids = await _httpClient.GetFromJsonAsync<List<int>>(_options.BestStoriesUrl);
            return ids ?? new List<int>();
        }

        public async Task<HackerNewsItem> GetItemAsync(int id)
        {
            var item = await _httpClient.GetFromJsonAsync<HackerNewsItem>(
                string.Format(_options.ItemUrlTemplate, id));

            if (item == null)
                throw new InvalidOperationException($"HackerNews item {id} returned null");

            return item;
        }
    }
}
