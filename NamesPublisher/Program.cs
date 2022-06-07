using Microsoft.Extensions.Configuration;
using System;

namespace NamesPublisher
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Initialize app
            var config = Initialize();


            //Read Name from User
            Console.WriteLine("Provide Name");
            var name = Console.ReadLine();

            var queueManager = new QueueManager(config);
            var connection = queueManager.CreateConnection();

            //Publish Name to RabbitMQ if not empty
            if (!string.IsNullOrEmpty(name))
            {
                var publish = new NamesPublisher();

                publish.SendMessage(name,queueManager.Queue,connection);
            }

                

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        private static IConfiguration Initialize()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)             
                .AddEnvironmentVariables();
            var configuration = builder.Build();

            return configuration;
        }
    }
}
