using HackerNews.Application.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace HackerNews.Infrastructure.RedisCaching;

public class RedisCacheService : ICacheService
{
    private readonly IDatabase _db;

    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _db.StringGetAsync(key);

        if (value.IsNullOrEmpty)
            return default;

        return JsonConvert.DeserializeObject<T>(value!);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var json = JsonConvert.SerializeObject(value);

        await _db.StringSetAsync(key, json, expiry);
    }
}