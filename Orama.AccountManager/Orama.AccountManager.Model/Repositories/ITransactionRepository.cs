using Orama.AccountManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Orama.AccountManager.Model.Repositories
{
    public interface ITransactionRepository : IRepository<FinancialTransaction>
    {

    }
}
