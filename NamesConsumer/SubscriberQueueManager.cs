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
        private const string QueueName = "NamesQueue";
        private const string ExchangeName = "DirectRoutingExchange";
        private string host;
        private string username;
        private string password;

        public SubscriberQueueManager(IConfiguration configuration)
        {
            _configuration = configuration;

             host = _configuration["RabbitMqHost"];
             username = _configuration["RabbitMqUsername"];
             password = _configuration["RabbitMqPassword"];
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
                    channel.QueueBind(QueueName, ExchangeName, "Name");

                    var consumer = new EventingBasicConsumer(channel);

                    while (true)
                    {
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body.ToArray();
                            var message = Encoding.UTF8.GetString(body);
                            Console.WriteLine($"Hello {message}, I am your father!");
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
