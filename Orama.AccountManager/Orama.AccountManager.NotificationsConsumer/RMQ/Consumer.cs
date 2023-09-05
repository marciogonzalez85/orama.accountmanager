using Newtonsoft.Json;
using Orama.AccountManager.Application.Commands.Transactions;
using Orama.AccountManager.CrossCutting;
using Orama.AccountManager.CrossCutting.Common;
using Orama.AccountManager.Model.Entities;
using Orama.AccountManager.Model.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orama.AccountManager.NotificationsConsumer.RMQ
{
    public class Consumer
    {
        private IConnection _connection;
        private IModel _channel;

        public void Start()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOSTNAME"), //"localhost", // RabbitMQ server hostname
                    UserName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME"), // "guest",     // RabbitMQ username
                    Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") // "guest"      // RabbitMQ password
                };

                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.QueueDeclare(queue: "notifications_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Log.Debug($"NotificationsConsumer: Received: {message}");

                    await ConsumeMessage(message);
                };

                _channel.BasicConsume(queue: "notifications_queue", autoAck: true, consumer: consumer);

                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception ex)
            {
                Log.Error($"NotificationsConsumer: Error: {ex.GetInnerException().Message}");
                throw;
            }
        }

        private async Task ConsumeMessage(string message)
        {
            try
            {
                var transaction = JsonConvert.DeserializeObject<Result<FinancialTransaction>>(message);

                using var client = new HttpClient();

                var uri = new Uri(transaction.Data.BankAccount.Cliente.WebhookUrl);

                client.BaseAddress = new Uri($"{uri.Scheme}://{uri.Host}:{uri.Port}");

                var notificationTransaction = new FinancialTransaction
                {
                    BankAccountId = transaction.Data.BankAccountId,
                    Date = transaction.Data.Date,
                    ExternalReference = transaction.Data.ExternalReference,
                    FinancialAssetId = transaction.Data.FinancialAssetId,
                    Id = transaction.Data.Id,
                    Quantity = transaction.Data.Quantity,
                    TotalValue = transaction.Data.TotalValue,
                    TransactionType = transaction.Data.TransactionType
                };

                var notificationBody = new
                {
                    Status = transaction.StatusCode,
                    Errors = transaction.Errors,
                    Success = transaction.Success,
                    Data = notificationTransaction
                };

                Log.Debug($"Notifying transaction: {JsonConvert.SerializeObject(notificationBody)}");

                var body = new StringContent(
                    JsonConvert.SerializeObject(notificationBody, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }), 
                    Encoding.UTF8, 
                    "application/json");

                await client.PostAsync(uri.AbsolutePath, body, CancellationToken.None);
            }
            catch (Exception ex)
            {
                Log.Error(ex.GetInnerException(), "Notifications consumer error");

                // Politica de retry/dead letter
            }
        }
    }
}
