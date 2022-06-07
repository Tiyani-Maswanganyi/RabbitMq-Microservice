using NamesConsumer;
using System;
using System.Text;
using Xunit;

namespace ConsumerTests
{
    public class MessagePrinterTests
    {
        [Fact]
        public void MessageOutput_Returns_ExpectedStringMessage()
        {
            //Arrange
            var sut = new MessagePrinter();
            string name = "Tiyani";
            byte[] arrName = Encoding.ASCII.GetBytes(name);

            //Act
            var result = sut.MessageOutput(arrName);

            //Assert
            Assert.Equal($"Hello {name}, I am your father!", result);
        }
    }
}