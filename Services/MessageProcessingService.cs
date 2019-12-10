using System;
using System.Collections.Generic;
using Chat.Models;
using System.Text.Json;
using Chat.Abstractions;

namespace Chat.Services
{
    public class MessageProcessingService : IMessageProcessingService
    {
        private static readonly Dictionary<string, ChatOperations> ChatOperations = new Dictionary<string, ChatOperations>()
        {
            { "join",  Models.ChatOperations.JOIN},
            { "publish",  Models.ChatOperations.PUBLISH},
            { "leave",  Models.ChatOperations.LEAVE}
        };

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public void Process(byte[] body)
        {
            try
            {
                var jsonString = System.Text.Encoding.UTF8.GetString(body);

                var messageIn = JsonSerializer.Deserialize<MessageIn>(jsonString, JsonOptions);

                var chatOperation = ChatOperations[messageIn.Type];
                switch (chatOperation)
                {
                    case Models.ChatOperations.JOIN:
                        Console.WriteLine($"User {messageIn.Nickname} just joined the chat");
                        break;
                    case Models.ChatOperations.LEAVE:
                        Console.WriteLine($"User {messageIn.Nickname} just left the chat");
                        break;
                    case Models.ChatOperations.PUBLISH:
                        Console.WriteLine($"{messageIn.Nickname}: {messageIn.Message}");
                        break;
                    default:
                        Console.WriteLine($"Not a Known operation: {chatOperation}");
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Not able to process the message");
                Console.WriteLine("Reason: " + e.Message);
            }
        }
    }
}
