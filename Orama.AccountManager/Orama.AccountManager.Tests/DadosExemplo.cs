using Orama.AccountManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orama.AccountManager.Tests
{
    public static class DadosExemplo
    {
        public static List<Cliente> Clientes { get; }
        public static List<BankAccount> Accounts { get; }
        public static List<FinancialAsset> Assets { get; }
        public static List<FinancialTransaction> Transactions { get; }

        static DadosExemplo()
        {
            #region Clientes
            var cliente1 = new Cliente
            {
                Id = 1,
                Name = "Cliente A",
                Email = "cliente_a@email.com",
                Password = "8F96F6FBF57047FF9FA2D07EB73C6019",
                WebhookUrl = "http://cliente_a_domain:8080/webhook/"
            };

            var cliente2 = new Cliente
            {
                Id = 2,
                Name = "Cliente B",
                Email = "cliente_b@email.com",
                Password = "A785F4EE07E8498CB06F40ACCB06ADAA",
                WebhookUrl = "http://cliente_b/notifications/"
            };

            Clientes = new List<Cliente> { cliente1, cliente2 };
            #endregion

            #region Accounts
            var account1 = new BankAccount
            {
                Id = 1,
                Balance = 1500,
                ClienteId = 1
            };

            var account2 = new BankAccount
            {
                Id = 2,
                Balance = 800,
                ClienteId = 2
            };

            Accounts = new List<BankAccount> { account1, account2 };
            #endregion

            #region Assets
            var asset1 = new FinancialAsset
            {
                Id = 1,
                Name = "AXZ Stock",
                Price = 15
            };

            var asset2 = new FinancialAsset
            {
                Id = 2,
                Name = "Asset xpto",
                Price = 35
            };

            Assets = new List<FinancialAsset> { asset1, asset2 };
            #endregion

            #region Transactions
            var transaction1 = new FinancialTransaction
            {
                Id = 1,
                BankAccountId = 1,
                Date = DateTime.Now,
                ExternalReference = "ExternalReference_transaction1",
                FinancialAssetId = 1,
                Quantity = 2,
                TransactionType = Model.Code.TransactionType.Buy,
                TotalValue = 30
            };

            var transaction2 = new FinancialTransaction
            {
                Id = 2,
                BankAccountId = 1,
                Date = DateTime.Now.AddDays(-2),
                ExternalReference = "ExternalReference_transaction2",
                FinancialAssetId = 1,
                Quantity = 1,
                TransactionType = Model.Code.TransactionType.Sell,
                TotalValue = 15
            };

            var transaction3 = new FinancialTransaction
            {
                Id = 3,
                BankAccountId = 2,
                Date = DateTime.Now.AddDays(-1),
                ExternalReference = "ExternalReference_transaction2",
                FinancialAssetId = 2,
                Quantity = 4,
                TransactionType = Model.Code.TransactionType.Buy,
                TotalValue = 140
            };

            Transactions = new List<FinancialTransaction> { transaction1, transaction2, transaction3 };
            #endregion

        }
    }
}
