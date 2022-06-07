using Moq;
using RabbitMQ.Client;
using System;
using System.Text;
using Xunit;

namespace NamesPublisher.Tests
{
    public class NamesPublisherTests
    {
        
        [Fact]
        public void SendMessage_Returns_SentByteArrayMessage()
        {
            //Arrange
            Mock<IModel> model = new Mock<IModel>();
            var sut = new NamesPublisher();
            string name = "Tiyani";
            string testQueue = "TestQueue";

            //Act
            var result = sut.SendMessage(name, testQueue, model.Object);

            //Assert        
            Assert.Equal(Encoding.ASCII.GetBytes(name),result);
        }
    }
}
