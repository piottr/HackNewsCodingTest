namespace HackerNews.Infrastructure.Config;

public record HackerNewsOptions
{
    public string BestStoriesUrl { get; set; } = default!;

    public string ItemUrlTemplate { get; set; } = default!;
}
