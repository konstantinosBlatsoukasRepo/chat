using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Chat.Abstractions;
using Chat.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Chat.Services
{
    public class ChatHostedService : IHostedService, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IConfiguration _configuration;
        private readonly IModel _channel;
        private readonly IMessageProcessingService _messageProcessingService;

        private const string MyName = "Kostas";
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public ChatHostedService(IQueueAndExchangeDeclarationService queueAndExchangeDeclarationService, IConfiguration configuration, IMessageProcessingService messageProcessingService)
        {
            _configuration = configuration;
            _connection = queueAndExchangeDeclarationService.GetConnection();
            _channel = queueAndExchangeDeclarationService.GetChannel();
            _messageProcessingService = messageProcessingService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ch, ea) => _messageProcessingService.Process(ea.Body);

            var queueName = _configuration.GetValue<string>("queueName");
            var exchangeName = _configuration.GetValue<string>("exchangeName");

            _channel.BasicConsume(queueName, true, consumer);

            var props = _channel.CreateBasicProperties();


            var keyboardInput = Console.ReadLine();

            while (keyboardInput != "quit")
            {
                var message = new MessageIn();
                message.Message = keyboardInput;
                message.Type = "publish";
                message.Nickname = MyName;
                message.Timestamp = (int) (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                var serializedMessage = JsonSerializer.Serialize(message, JsonOptions);
                var messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(serializedMessage);

                _channel.BasicPublish(exchangeName,
                    "", props,
                    messageBodyBytes);

                keyboardInput = Console.ReadLine();
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
