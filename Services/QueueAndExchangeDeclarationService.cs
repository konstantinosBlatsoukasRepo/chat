using Chat.Abstractions;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Chat.Services
{
    public class QueueAndExchangeDeclarationService : IQueueAndExchangeDeclarationService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public QueueAndExchangeDeclarationService(IQueueConnectionService queueConnectionService, IConfiguration configuration)
        {
            _connection = queueConnectionService.GetConnection();
            _channel = _connection.CreateModel();

            var queueName = configuration.GetValue<string>("queueName");
            var exchangeName = configuration.GetValue<string>("exchangeName");

            //_channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout);
            _channel.QueueDeclare(queueName, true, true, true, null);

            _channel.QueueBind(queueName, exchangeName, "", null);
        }

        public IModel GetChannel() => _channel;

        public IConnection GetConnection() => _connection;
    }
}
