using HackerNews.Infrastructure.RabbitMQ.Config;
using HackerNews.Application.Interfaces;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using HackerNews.Application.Consumers;

namespace HackerNews.Infrastructure.RabbitMQ.Extensions;

public static class Installer
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqOptions>(configuration.GetSection("RabbitMq"));

        services.AddMassTransit(x =>
        {
            x.AddConsumer<BestStoriesUpdatedConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                var options = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;

                cfg.Host(options.HostName, options.VirtualHost, h =>
                {
                    h.Username(options.UserName);
                    h.Password(options.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddScoped<IMessageBus, MassTransitMessageBus>();

        return services;
    }
}