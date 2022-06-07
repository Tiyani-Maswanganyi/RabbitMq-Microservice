using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Configuration;
using System.Text;


namespace NamesPublisher
{
    public class QueueManager
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _model;
        private static IConfiguration _configuration;

        private const string QueueName = "NamesQueue";
        private const string ExchangeName = "DirectRoutingExchange";
        private const string RoutingKey = "Name";

        public QueueManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public IModel CreateConnection()
        {
            string host = _configuration["RabbitMqHost"];
            string username = _configuration["RabbitMqUsername"];
            string password = _configuration["RabbitMqPassword"];

            _factory = new ConnectionFactory { HostName = host, UserName = username, Password = password };
            _connection = _factory.CreateConnection();
            _model = _connection.CreateModel();
            _model.ExchangeDeclare(ExchangeName, "direct");
            _model.QueueDeclare(QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _model.QueueBind(QueueName, ExchangeName, RoutingKey);
       
            return _model;
        }

        public string Queue {
            get{ return QueueName; } 
        }

   
    }
}
