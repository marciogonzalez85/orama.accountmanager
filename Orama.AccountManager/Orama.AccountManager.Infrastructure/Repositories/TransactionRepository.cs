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
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AccountManagerDbContext _db;

        public TransactionRepository(AccountManagerDbContext db)
        {
            _db = db;
        }

        public async Task<FinancialTransaction> AddAsync(FinancialTransaction item, CancellationToken cancellationToken)
        {
            item.Date = DateTime.Now;

            await _db.FinancialTransactions.AddAsync(item, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

            return item;
        }

        public Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<FinancialTransaction>> GetAllAsync(CancellationToken cancellationToken) =>
            (await _db.FinancialTransactions.Include(a => a.FinancialAsset).ToListAsync(cancellationToken));


        public async Task<FinancialTransaction> GetAsync(int id, CancellationToken cancellationToken) =>
            await _db.FinancialTransactions.Include(a => a.BankAccount).ThenInclude(a => a.Cliente).SingleOrDefaultAsync(a => a.Id == id, cancellationToken);
        

        public Task UpdateAsync(FinancialTransaction item, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
