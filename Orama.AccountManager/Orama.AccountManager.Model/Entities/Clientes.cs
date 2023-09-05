using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orama.AccountManager.Model.Entities
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        
        public string WebhookUrl { get; set; }

        public virtual ICollection<BankAccount> BankAccounts { get; set; }
    }
}
