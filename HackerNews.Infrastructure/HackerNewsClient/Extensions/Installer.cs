using HackerNews.Infrastructure.Config;
using HackerNews.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HackerNews.Infrastructure.HackerNewsClient.Extensions;

public static class Installer
{
    public static IServiceCollection AddHackerNewsClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<HackerNewsOptions>(configuration.GetSection("HackerNews"));

        services.AddHttpClient<IHackerNewsService, HackerNewsService>();

        return services;
    }
}