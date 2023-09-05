using Orama.AccountManager.Model.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orama.AccountManager.Model.Entities
{
    public class FinancialTransaction
    {      
        public int Id { get; set; }
        public int BankAccountId { get; set; }
        public int FinancialAssetId { get; set; }
        public TransactionType TransactionType { get; set; }
        public int Quantity { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime Date { get; set; }
        public string ExternalReference { get; set; }

        public virtual BankAccount BankAccount { get; set; }
        public virtual FinancialAsset FinancialAsset { get; set; }
    }
}
