using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Orama.AccountManager.Model.Entities
{
    public  class BankAccount
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public decimal Balance { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual ICollection<FinancialTransaction> FinancialTransactions { get; set; }
    }
}
