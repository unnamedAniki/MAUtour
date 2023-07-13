using MAUtour.Local.Repository.Interfaces;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MAUtour.Local.DBConnect;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MAUtour.Local.Repository
{
    internal class Repository<T> : IRepository<T> where T : class
    {
        private LocalContext _context;
        protected readonly DbSet<T> DbSets;
        public Repository(LocalContext context)
        {
            this._context = context ?? throw new ArgumentNullException();
            DbSets = context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await DbSets.AsNoTracking().ToListAsync();
        }
        public async ValueTask<T> GetByIdAsync(int id)
        {
            return await DbSets.FindAsync(id);
        }
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSets.AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSets.AsNoTracking().SingleOrDefaultAsync(predicate);
        }
        public async Task<bool> AddAsync(T entity)
        {
            try
            {
                await DbSets.AddAsync(entity);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddRangeAsync(IEnumerable<T> entities)
        {
            try
            {
                await DbSets.AddRangeAsync(entities);
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public void Update(T entity)
        {
            DbSets.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Update(entity);
            }
        }

        public void Remove(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
                DbSets.Attach(entity);
            DbSets.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                if (_context.Entry(entity).State == EntityState.Detached)
                    DbSets.Attach(entity);
                DbSets.Remove(entity);
            }
        }
        
        public IIncludableQueryable<T, IProperty> Include(Expression<Func<T, IProperty>> predicate)
        {
            return DbSets.Include(predicate);
        }

    }
}
