using HackerNews.Contracts.Cache;
using HackerNews.Contracts.IntegrationEvents;
using HackerNews.Application.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace HackerNews.Application.Consumers;

public class BestStoriesUpdatedConsumer : IConsumer<BestStoriesRankingUpdatedIntegrationEvent>
{
    private readonly ICacheService _cacheService;
    private readonly ILogger<BestStoriesUpdatedConsumer> _logger;

    public BestStoriesUpdatedConsumer(ICacheService cacheService, ILogger<BestStoriesUpdatedConsumer> logger)
    {
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<BestStoriesRankingUpdatedIntegrationEvent> context)
    {
        var @event = context.Message;
        
        _logger.LogInformation("Received BestStoriesRankingUpdatedIntegrationEvent. Updating cache with {Count} stories.", @event.BestStoriesIds.Count);
         
        await _cacheService.SetAsync(
            CacheKeys.BestStoriesPrefix, 
            @event.BestStoriesIds, 
            TimeSpan.FromMinutes(10));
            
        _logger.LogInformation("Successfully updated Redis cache for best_stories_ranking.");
    }
}
