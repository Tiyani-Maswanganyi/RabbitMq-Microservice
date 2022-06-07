using System;
using System.Collections.Generic;
using System.Text;

namespace NamesConsumer
{
    public class MessagePrinter : IMessagePrinter
    {

        public string MessageOutput(byte[] arrName)
        {
            var name = Encoding.UTF8.GetString(arrName);
            var consoleOutput = $"Hello {name}, I am your father!";
            Console.WriteLine(consoleOutput);
            return consoleOutput;
        }
    }
}
