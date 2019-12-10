using Chat.Abstractions;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Chat.Services
{
    public class QueueConnectionService : IQueueConnectionService
    {
        private readonly IConfiguration _configuration;

        public QueueConnectionService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConnection GetConnection()
        {
            var factory = new ConnectionFactory {AutomaticRecoveryEnabled = true};

            var userName = _configuration.GetValue<string>("username");
            var password = _configuration.GetValue<string>("password");
            var hostName = _configuration.GetValue<string>("hostName");

            factory.UserName = userName;
            factory.Password= password;
            factory.HostName = hostName;

            var connection = factory.CreateConnection();
            return connection;
        }
    }
}
