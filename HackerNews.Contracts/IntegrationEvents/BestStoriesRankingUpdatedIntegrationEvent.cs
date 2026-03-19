using System;
using System.Collections.Generic;

namespace HackerNews.Contracts.IntegrationEvents;

public record BestStoriesRankingUpdatedIntegrationEvent
{
    public IReadOnlyList<int> BestStoriesIds { get; init; } = Array.Empty<int>();
    public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;
}