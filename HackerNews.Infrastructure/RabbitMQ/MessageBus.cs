using HackerNews.Application.Interfaces;
using MassTransit;

namespace HackerNews.Infrastructure.RabbitMQ;

public class MassTransitMessageBus : IMessageBus
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitMessageBus(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
    {
        return _publishEndpoint.Publish(message!, cancellationToken);
    }
}