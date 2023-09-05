using Orama.AccountManager.Model.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orama.AccountManager.Application.DTO
{
    public class TransactionDTO
    {
        public int Id { get; set; }
        public int AccountID { get; set; }
        public int AssetID { get; set; }
        public string AssetName { get; set; }
        public TransactionType TransactionType { get; set; }
        public int Quantity { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime Date { get; set; }
    }
}
