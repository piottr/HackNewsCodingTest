using HackerNews.Contracts.IntegrationEvents;
using HackerNews.Application.Interfaces;
using HackerNews.Application.Dto; 

namespace HackerNews.SyncWorker;

public class HackerNewsWorker : BackgroundService
{
    private readonly IHackerNewsService _hnService;
    private readonly IMessageBus _messageBus;
     
    private List<int> _lastBestStories = new List<int>();

    public HackerNewsWorker(IHackerNewsService hnService, IMessageBus messageBus)
    {
        _hnService = hnService;
        _messageBus = messageBus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        { 
            var currentBestStories = await _hnService.GetBestStoryIdsAsync();
             
            if (!_lastBestStories.SequenceEqual(currentBestStories))
            {
                var rankingEvent = new BestStoriesRankingUpdatedIntegrationEvent
                {
                    BestStoriesIds = currentBestStories,
                    UpdatedAt = DateTime.UtcNow
                };

                await _messageBus.PublishAsync(rankingEvent);
                _lastBestStories = currentBestStories;
            }

            var items = await FetchItemsWithLimitAsync(currentBestStories, 10);

            foreach (var article in items)
            {  
                if (IsUpdated(article))
                {
                    var detailsEvent = new StoryDetailsFetchedIntegrationEvent
                    {
                        Id = article.Id,
                        Title = article.Title,
                        Type = article.Type,
                        Url = article.Url,
                        PostedBy = article.By,
                        PostedAt = DateTimeOffset.FromUnixTimeSeconds(article.Time ?? 0).UtcDateTime
                    };

                    await _messageBus.PublishAsync(detailsEvent);
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }

    private async Task<List<HackerNewsItemDto>> FetchItemsWithLimitAsync(IEnumerable<int> ids, int maxConcurrency)
    {
        var semaphore = new SemaphoreSlim(maxConcurrency);
        var tasks = ids.Select(async id =>
        {
            await semaphore.WaitAsync();
            try
            {
                return await _hnService.GetItemAsync(id);
            }
            finally
            {
                semaphore.Release();
            }
        });

        return (await Task.WhenAll(tasks)).ToList();
    }

    private bool IsUpdated(HackerNewsItemDto article)
    {
        return true;
    }
}