using RabbitMQ.Client;

namespace Chat.Abstractions
{
    public interface IQueueAndExchangeDeclarationService
    {
        IModel GetChannel();

        IConnection GetConnection();
    }
}
