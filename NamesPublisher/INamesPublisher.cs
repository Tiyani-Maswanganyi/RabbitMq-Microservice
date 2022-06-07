using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace NamesPublisher
{
    public interface INamesPublisher
    {
        byte[] SendMessage(string message, string queueName, IModel model);
    }
}
