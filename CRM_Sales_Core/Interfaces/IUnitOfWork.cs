using CRM_Sales_Core.Entites;

namespace CRM_Sales_Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Team> Teams { get; }
        IGenericRepository<SalesAgent> SalesAgents { get; }
        IGenericRepository<Project> Projects { get; }
        IGenericRepository<Client> Clients { get; }
        Task<int> SaveChangesAsync();
    }
}
