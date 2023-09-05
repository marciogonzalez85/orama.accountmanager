using Orama.AccountManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orama.AccountManager.Tests
{
    public class Fixture : IDisposable
    {
        private readonly List<Cliente> _clientes = new List<Cliente>();
        private readonly List<BankAccount> _accounts = new List<BankAccount>();
        private readonly List<FinancialAsset> _assets = new List<FinancialAsset>();
        private readonly List<FinancialTransaction> _transactions = new List<FinancialTransaction>();

        public IReadOnlyCollection<Cliente> Clientes => _clientes;
        public IReadOnlyCollection<BankAccount> BankAccounts => _accounts;
        public IReadOnlyCollection<FinancialAsset> FinancialAssets => _assets;
        public IReadOnlyCollection<FinancialTransaction> Transactions => _transactions;

        public Fixture()
        {
            _clientes.AddRange(DadosExemplo.Clientes);
            _accounts.AddRange(DadosExemplo.Accounts);
            _assets.AddRange(DadosExemplo.Assets);
            _transactions.AddRange(DadosExemplo.Transactions);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
