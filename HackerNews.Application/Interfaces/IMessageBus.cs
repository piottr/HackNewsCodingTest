namespace HackerNews.Application.Interfaces;
 
public interface IMessageBus
{
    Task PublishAsync<T>(T message, CancellationToken cancellationToken = default);
}