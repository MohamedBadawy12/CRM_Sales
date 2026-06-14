using CRM_Sales_Core.Entites;
using CRM_Sales_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CRM_Sales_Infrastructure.Repositories
{
    public class SalesAgentRepository : GenericRepository<SalesAgent>
    {
        public SalesAgentRepository(AppDbContext context) : base(context) { }

        public override async Task<IEnumerable<SalesAgent>> GetAllWithIncludesAsync()
            => await _dbSet
                .Where(x => !x.IsDeleted)
                .Include(a => a.Team)
                .Include(a => a.Leader)
                .AsNoTracking()
                .ToListAsync();
    }
}
