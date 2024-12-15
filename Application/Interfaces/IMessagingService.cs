namespace Application.Interfaces;

public interface IMessagingService
{
     Task PublishMessageAsync<T>(string queueName, T message);
}