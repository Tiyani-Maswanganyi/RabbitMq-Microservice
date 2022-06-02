using Microsoft.Extensions.Configuration;
using System;

namespace NamesConsumer
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Initialize app
            var config = Initialize();

            var subscriber = new SubscriberQueueManager(config);

            // listen for messages
            subscriber.RecieveMessages();
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
