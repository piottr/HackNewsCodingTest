using System;
using System.Collections.Generic;
using System.Text;

namespace HackerNews.Contracts.IntegrationEvents;

public class StoryDetailsFetchedIntegrationEvent
{
    public int Id { get; set; }
    public string? Title { get; set; } = string.Empty;
    public string? Type { get; set; } = string.Empty;
    public string? Url { get; set; } = string.Empty;
    public string? PostedBy { get; set; } = string.Empty;
    public DateTime PostedAt { get; set; }
}