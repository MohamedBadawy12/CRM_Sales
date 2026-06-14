using CRM_Sales_Core.Entites;
using CRM_Sales_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CRM_Sales_Infrastructure.Repositories
{
    public class ClientRepository : GenericRepository<Client>
    {
        public ClientRepository(AppDbContext context) : base(context) { }

        public override async Task<IEnumerable<Client>> GetAllWithIncludesAsync()
            => await _dbSet
                .Where(x => !x.IsDeleted)
                .Include(c => c.Project)
                .Include(c => c.Agent)
                .Include(c => c.PreviousAgent)
                .AsNoTracking()
                .ToListAsync();
    }
}
