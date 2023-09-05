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
    public class AssetRepository : IAssetRepository
    {
        private readonly AccountManagerDbContext _db;

        public AssetRepository(AccountManagerDbContext db)
        {
            _db = db;
        }

        public Task<FinancialAsset> AddAsync(FinancialAsset item, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FinancialAsset>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<FinancialAsset?> GetAsync(int id, CancellationToken cancellationToken)
        {
            return await _db.FinancialAssets.SingleOrDefaultAsync(a => a.Id == id, cancellationToken);
        }

        public Task UpdateAsync(FinancialAsset item, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
