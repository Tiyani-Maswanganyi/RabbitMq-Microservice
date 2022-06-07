using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace NamesPublisher
{
    public class NamesPublisher : INamesPublisher
    {

        public byte[] SendMessage(string message, string queueName, IModel model)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(message);

            model.BasicPublish("", queueName, null, bytes);

            return bytes;
        }
    }
}