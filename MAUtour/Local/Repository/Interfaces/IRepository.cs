using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace MAUtour.Local.Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        ValueTask<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate);
        IIncludableQueryable<T, IProperty> Include(Expression<Func<T, IProperty>> predicate);
        Task<bool> AddAsync(T entity);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        Task<bool> AddRangeAsync(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
