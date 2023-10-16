using RabbitMQ.Client;
using System;

namespace SomeBlog.RabbitMQClient
{
    public interface IRabbitMQService
    {
        IConnection CreateChannel();
    }

    public class RabbitMQService : IRabbitMQService
    {
        public IConnection CreateChannel()
        {
            var connectionFactory = new ConnectionFactory();
            connectionFactory.DispatchConsumersAsync = true;
            connectionFactory.Uri = new Uri("amqp://********:*******@158.69.25.***:5672/");

            var connection = connectionFactory.CreateConnection();

            return connection;
        }
    }
}
