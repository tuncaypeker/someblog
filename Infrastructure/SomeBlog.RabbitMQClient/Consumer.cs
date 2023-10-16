using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SomeBlog.RabbitMQClient
{
    public class Consumer<T>
    {
        private readonly IRabbitMQService _rabbitMQService;
        private string _queueName;

        public Consumer(string queueName, IRabbitMQService rabbitMqService)
        {
            _rabbitMQService = rabbitMqService;
            _queueName = queueName;
        }

        public void ConsumeString()
        {
            var connection = _rabbitMQService.CreateChannel();
            var channel = connection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine(message);
            };

            channel.BasicConsume(_queueName, autoAck: true, consumer);
        }

        public void ConsumeObject(AsyncEventHandler<BasicDeliverEventArgs> receivedEvent)
        {
            var connection = _rabbitMQService.CreateChannel();
            var channel = connection.CreateModel();

            var consumer = new AsyncEventingBasicConsumer(channel);
            
            consumer.Received += receivedEvent;

            channel.BasicConsume(_queueName, true, consumer);
        }

        /*
         
        public void ConsumeObject(EventHandler<BasicDeliverEventArgs> receivedEvent)
        {
            var connection = _rabbitMQService.CreateChannel();
            var channel = connection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += receivedEvent;

            channel.BasicConsume(_queueName, true, consumer);
        }

         */
    }
}
