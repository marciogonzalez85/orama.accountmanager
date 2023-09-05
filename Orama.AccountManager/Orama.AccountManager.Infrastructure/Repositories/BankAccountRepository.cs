using Microsoft.EntityFrameworkCore;
using Orama.AccountManager.Infrastructure.DB;
using Orama.AccountManager.Model.Entities;
using Orama.AccountManager.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orama.AccountManager.Infrastructure.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly AccountManagerDbContext _db;

        public BankAccountRepository(AccountManagerDbContext db)
        {
            _db = db;
        }

        public Task<BankAccount> AddAsync(BankAccount item, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<BankAccount?> GetAsync(int id, CancellationToken cancellationToken) =>
            await _db.BankAccounts.SingleOrDefaultAsync(a => a.Id == id, cancellationToken);


        public async Task<IEnumerable<BankAccount>> GetAllAsync(CancellationToken cancellation) =>
            (await _db.BankAccounts.ToListAsync(cancellation));

        public async Task UpdateAsync(BankAccount item, CancellationToken cancellationToken)
        {
            var entity = await _db.BankAccounts.SingleOrDefaultAsync(a => a.Id == item.Id, cancellationToken);

            entity.Balance = item.Balance;

            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
