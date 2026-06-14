using CRM_Sales_Core.Entites;
using CRM_Sales_Core.Interfaces;
using CRM_Sales_Infrastructure.Data;
using CRM_Sales_Infrastructure.Repositories;

namespace CRM_Sales_Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IGenericRepository<Team> Teams { get; private set; }
        public IGenericRepository<SalesAgent> SalesAgents { get; private set; }
        public IGenericRepository<Project> Projects { get; private set; }
        public IGenericRepository<Client> Clients { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Teams = new GenericRepository<Team>(_context);
            SalesAgents = new SalesAgentRepository(_context);
            Projects = new GenericRepository<Project>(_context);
            Clients = new ClientRepository(_context);
        }

        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();

        public void Dispose()
            => _context.Dispose();
    }
}
