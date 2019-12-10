using RabbitMQ.Client;

namespace Chat.Abstractions
{
    public interface IQueueConnectionService
    {
        IConnection GetConnection();
    }
}
