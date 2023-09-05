using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orama.AccountManager.Infrastructure.RMQ
{
    public class MsgProducer
    {
        public static void PublishTransactionMessage(string message)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOSTNAME"), //"localhost", // RabbitMQ server hostname
                    UserName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME"), // "guest",     // RabbitMQ username
                    Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") // "guest"      // RabbitMQ password
                };

                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.QueueDeclare(queue: "transactions_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "", routingKey: "transactions_queue", basicProperties: null, body: body);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
