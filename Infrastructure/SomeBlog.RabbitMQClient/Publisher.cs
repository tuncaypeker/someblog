using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace SomeBlog.RabbitMQClient
{
    public class Publisher<T>
    {
        private readonly IRabbitMQService _rabbitMQService;
        private string _queueName;

        public Publisher(string queueName, IRabbitMQService rabbitMqService)
        {
            _rabbitMQService = rabbitMqService;
            _queueName = queueName;
        }

        public bool Publish(string message)
        {
            try
            {
                using (var connection = _rabbitMQService.CreateChannel())
                {
                    using (var model = connection.CreateModel())
                    {
                        model.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false, null);
                        model.BasicPublish("", _queueName, null, Encoding.UTF8.GetBytes(message));
                    }
                }
            }
            catch (Exception exc)
            {
                return false;
            }

            return true;
        }

        public bool Publish(T t)
        {
            try
            {
                using (var connection = _rabbitMQService.CreateChannel())
                {
                    using (var model = connection.CreateModel())
                    {
                        string jsonified = JsonConvert.SerializeObject(t);

                        model.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false, null);
                        model.BasicPublish("", _queueName, null, Encoding.UTF8.GetBytes(jsonified));
                    }
                }
            }
            catch (Exception exc)
            {
                return false;
            }

            return true;
        }
    }
}
