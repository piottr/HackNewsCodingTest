using System;
using System.Collections.Generic;
using System.Text;

namespace HackerNews.Contracts.IntegrationEvents;

public record StoryDetailsFetchedIntegrationEvent
{
    public int Id { get; init; }
    public string? Title { get; init; } = string.Empty;
    public string? Type { get; init; } = string.Empty;
    public string? Url { get; init; } = string.Empty;
    public string? PostedBy { get; init; } = string.Empty;
    public DateTime PostedAt { get; init; }
}