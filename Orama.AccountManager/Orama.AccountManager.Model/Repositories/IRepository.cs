using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orama.AccountManager.Model.Repositories
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
        Task<T> GetAsync(int id, CancellationToken cancellationToken);
        Task<T> AddAsync(T item, CancellationToken cancellationToken);
        Task UpdateAsync(T item, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
