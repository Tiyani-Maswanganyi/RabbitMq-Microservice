using System;
using System.Collections.Generic;
using System.Text;

namespace NamesConsumer
{
    public interface IMessagePrinter
    {
        string MessageOutput(byte[] arrName);
    }
}
