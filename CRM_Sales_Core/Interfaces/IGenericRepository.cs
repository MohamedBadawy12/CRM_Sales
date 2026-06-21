namespace CRM_Sales_Core.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllWithIncludesAsync();
        Task<IEnumerable<T>> GetAllDeletedAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task<T?> GetByIdIncludingDeletedAsync(Guid id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id); // Soft Delete
        Task RestoreAsync(Guid id);
        Task HardDeleteAsync(Guid id);
    }
}
