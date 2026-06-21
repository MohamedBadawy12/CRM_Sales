using CRM_Sales_Core.Entites;
using CRM_Sales_Core.Interfaces;
using CRM_Sales_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CRM_Sales_Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _dbSet.Where(x => !x.IsDeleted).AsNoTracking().ToListAsync();

        public virtual async Task<IEnumerable<T>> GetAllWithIncludesAsync()
            => await _dbSet.Where(x => !x.IsDeleted).ToListAsync();

        public virtual async Task<IEnumerable<T>> GetAllDeletedAsync()
           => await _dbSet.Where(x => x.IsDeleted).AsNoTracking().ToListAsync();

        public async Task<T?> GetByIdAsync(Guid id)
            => await _dbSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        public async Task<T?> GetByIdIncludingDeletedAsync(Guid id)
            => await _dbSet.FirstOrDefaultAsync(x => x.Id == id);

        public async Task AddAsync(T entity)
            => await _dbSet.AddAsync(entity);

        public async Task UpdateAsync(T entity)
        {
            entity.SetUpdatedAt();
            _dbSet.Update(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
                entity.SoftDelete();
        }

        public async Task RestoreAsync(Guid id)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted);
            if (entity != null)
                entity.Restore();
        }

        public async Task HardDeleteAsync(Guid id)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted);
            if (entity != null)
                _dbSet.Remove(entity);
        }
    }
}
