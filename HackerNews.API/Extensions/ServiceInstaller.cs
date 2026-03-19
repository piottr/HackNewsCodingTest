using HackerNews.API.Services;
using HackerNews.API.Services.Interfaces;

namespace HackerNews.Infrastructure.Extensions;

public static class ServiceInstaller
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IBestStoriesService, BestStoriesService>();

        return services;
    }
}