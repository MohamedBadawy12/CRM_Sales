namespace CRM_Sales_Core.Entites
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; protected set; }
        public bool IsDeleted { get; protected set; } = false;

        public void SetUpdatedAt() => UpdatedAt = DateTime.UtcNow;
        public void SoftDelete() => IsDeleted = true;
    }
}
