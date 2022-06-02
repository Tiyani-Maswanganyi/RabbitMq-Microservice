using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Configuration;
using System.Text;


namespace Publisher
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
        public QueueManager()
        {
            _configuration = Initialise();

            string host = _configuration["Host"];
            string username = _configuration["UserName"];
            string password = _configuration["Password"];

            CreateConnection(host,username,password);
        }


        private IConfiguration Initialise()
        {

            var test = Environment.GetEnvironmentVariables();
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                .AddEnvironmentVariables();
            var configuration = builder.Build();

            return configuration;
        }

        private static void CreateConnection(string host,string username,string password)
        {
            _factory = new ConnectionFactory { HostName = host, UserName = username, Password = password };
            _connection = _factory.CreateConnection();
            _model = _connection.CreateModel();
            _model.ExchangeDeclare(ExchangeName, "direct");
            _model.QueueDeclare(QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _model.QueueBind(QueueName, ExchangeName, RoutingKey);
        }

        public void SendMessage(string message)
        {     
            byte[] bytes = Encoding.ASCII.GetBytes(message);
            var props = _model.CreateBasicProperties();
            props.Persistent = true;

            _model.BasicPublish("", QueueName, null, bytes);
      
        }
    }
}
