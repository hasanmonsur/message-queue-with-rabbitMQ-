using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Microsoft.Extensions.Configuration;
using MessageQueueApp.Models;
using MessageQueueApp.Contacts;
using System.Text.Json;

namespace MessageQueueApp.Services
{
    public class RabbitMQService : IMessageQueueService
    {
        private readonly string _hostName;
        private readonly string _queueName;
        private readonly IConfiguration _config;

        private readonly IMessageRepository _repository;

        public RabbitMQService(IConfiguration config, IMessageRepository repository)
        {
            _config = config;
            _hostName = _config["RabbitMQ:HostName"];
            _queueName = _config["RabbitMQ:QueueName"];

            _repository = repository;
        }

        public void PublishMessage(string message)
        {
            var factory = new ConnectionFactory() { HostName = _hostName };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "",
                                 routingKey: _queueName,
                                 basicProperties: null,
                                 body: body);
        }

        public void ConsumeMessages()
        {
            var factory = new ConnectionFactory() { HostName = _hostName };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            var message1 = "";

            consumer.Received += async (model, ea) =>
            {
                var  body = ea.Body.ToArray(); // Use Span for performance in .NET 8
                message1 = Encoding.UTF8.GetString(body.ToArray());
                //var message = Encoding.UTF8.GetString(body);
                await _repository.SaveMessageAsync(message1);
            };


            channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
        }
    }
}
