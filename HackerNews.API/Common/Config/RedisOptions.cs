namespace HackerNews.API.Common.Config
{
    public record RedisOptions
    {
        public string Connection { get; set; } = default!;
    }
}
