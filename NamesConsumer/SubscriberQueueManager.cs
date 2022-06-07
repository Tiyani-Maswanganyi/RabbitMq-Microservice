using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace NamesConsumer
{
    public class SubscriberQueueManager
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IConfiguration _configuration;
        private IMessagePrinter _messagePrinter;
        private const string QueueName = "NamesQueue";
        private const string ExchangeName = "DirectRoutingExchange";
        private const string RoutingKey = "Name";
        private string host;
        private string username;
        private string password;

        public SubscriberQueueManager(IConfiguration configuration)
        {
             _configuration = configuration;

             host = _configuration["RabbitMqHost"];
             username = _configuration["RabbitMqUsername"];
             password = _configuration["RabbitMqPassword"];
            _messagePrinter = new MessagePrinter();
        }

        public void RecieveMessages()
        {

            _factory = new ConnectionFactory { HostName = host, UserName = username, Password = password };

            using (_connection = _factory.CreateConnection())
            {
                using (var channel = _connection.CreateModel())
                {
                    channel.ExchangeDeclare(ExchangeName, "direct");
                    channel.QueueDeclare(queue: QueueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
                    channel.QueueBind(QueueName, ExchangeName, RoutingKey);

                    var consumer = new EventingBasicConsumer(channel);

                    while (true)
                    {
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body.ToArray();
                            _messagePrinter.MessageOutput(body);
                        };

                        channel.BasicConsume(queue: QueueName,
                                             autoAck: true,
                                             consumer: consumer);

                        Console.WriteLine(" Press [enter] to exit.");
                        Console.ReadLine();
                    }
                }
            }
        }
     
    }
}
