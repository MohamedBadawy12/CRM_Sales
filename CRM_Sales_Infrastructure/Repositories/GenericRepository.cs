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
            => await _dbSet.Where(x => !x.IsDeleted).ToListAsync();

        public async Task<T?> GetByIdAsync(Guid id)
            => await _dbSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

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
                entity.SoftDelete(); // Soft Delete مش بيمسح من DB
        }
    }
}
