using RabbitMQ.Client;

namespace NewsManagementService.Interfaces.RabbitMQ;

public interface IQueueConnection
{
    Task<IChannel> CreateChannelAsync();
}