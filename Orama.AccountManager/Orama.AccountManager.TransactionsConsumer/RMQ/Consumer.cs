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

namespace Orama.AccountManager.TransactionsConsumer.RMQ
{
    public class Consumer
    {
        private IAssetRepository _assetRepository;
        private ITransactionRepository _transactionRepository;
        private IBankAccountRepository _bankAccountRepository;
        private IConnection _connection;
        private IModel _channel;

        public Consumer(IAssetRepository assetRepository, ITransactionRepository transactionRepository, IBankAccountRepository bankAccountRepository)
        {
            _assetRepository = assetRepository;
            _transactionRepository = transactionRepository;
            _bankAccountRepository = bankAccountRepository;
        }

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

                _channel.QueueDeclare(queue: "transactions_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(_channel);

                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Log.Debug($"TransactionsConsumer: Received: {message}");

                    var transactionResult = await ConsumeMessage(message);

                    if (!transactionResult.Success) return;

                    #region Notificação
                    var dbTransaction = await _transactionRepository.GetAsync(transactionResult.Data.Id, CancellationToken.None);
                    if (dbTransaction == null) return;

                    var dadosCliente = dbTransaction.BankAccount.Cliente;

                    _channel.QueueDeclare(queue: "notifications_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

                    var notificationBody = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(transactionResult, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore}));
                    _channel.BasicPublish(exchange: "", routingKey: "notifications_queue", basicProperties: null, body: notificationBody);
                    #endregion
                };

                _channel.BasicConsume(queue: "transactions_queue", autoAck: true, consumer: consumer);

                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception ex)
            {
                Log.Error($"TransactionsConsumer: Error: {ex.GetInnerException().Message}");
                throw;
            }
        }

        private async Task<Result<FinancialTransaction>> ConsumeMessage(string message)
        {
            try
            {
                var command = JsonConvert.DeserializeObject<CreateTransactionCommand>(message);

                var asset = await _assetRepository.GetAsync(command.AssetID, CancellationToken.None);

                if (asset == null)
                    return Result<FinancialTransaction>.BadRequest("ativo", "Ativo informado não encontrado");

                var account = await _bankAccountRepository.GetAsync(command.AccountID, CancellationToken.None);

                if (account == null)
                    return Result<FinancialTransaction>.BadRequest("conta", "Conta informada não encontrada");

                var transaction = new FinancialTransaction
                {
                    BankAccountId = command.AccountID,
                    FinancialAssetId = command.AssetID,
                    TransactionType = command.TransactionType,
                    Quantity = command.Quantity,
                    TotalValue = asset.Price * command.Quantity,
                    ExternalReference = command.ExternalReference
                };

                var createResult = await _transactionRepository.AddAsync(transaction, CancellationToken.None);

                #region Update account balance
                var valueToUpdateBalance = command.TransactionType == Model.Code.TransactionType.Sell ?
                    transaction.TotalValue : (transaction.TotalValue * -1);

                account.Balance += valueToUpdateBalance;

                await _bankAccountRepository.UpdateAsync(account, CancellationToken.None);
                #endregion

                return Result<FinancialTransaction>.Ok(createResult);
            }
            catch (Exception ex)
            {
                Log.Error(ex.GetInnerException(), "Transactions consumer error");

                throw;

                // Exception handler: dead letter, retry ....
            }
        }
    }
}
