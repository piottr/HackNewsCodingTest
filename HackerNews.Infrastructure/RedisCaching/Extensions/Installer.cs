using HackerNews.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace HackerNews.Infrastructure.RedisCaching.Extensions;

public static class Installer
{
    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(configuration.GetSection("Redis:Connection").Value!));

        services.AddScoped<ICacheService, RedisCacheService>();

        return services;
    }
}