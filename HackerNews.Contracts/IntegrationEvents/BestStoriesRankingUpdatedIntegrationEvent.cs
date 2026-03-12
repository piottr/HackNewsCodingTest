namespace HackerNews.Contracts.IntegrationEvents;

public class BestStoriesRankingUpdatedIntegrationEvent
{
    public List<int> BestStoriesIds { get; set; } = new List<int>();
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}