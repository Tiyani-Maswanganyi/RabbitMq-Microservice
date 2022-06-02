using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Consumer
{
    public class SubscriberQueueManager
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;

        private const string QueueName = "NamesQueue";
        private const string ExchangeName = "DirectRoutingExchange";

        public SubscriberQueueManager()
        {
            RecieveMessages();
        }

        public void RecieveMessages()
        {
      
            _factory = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest" };

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

                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                       var  message = Encoding.UTF8.GetString(body);
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
