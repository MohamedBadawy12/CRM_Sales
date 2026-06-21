namespace CRM_Sales_Core.Entites
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; protected set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; protected set; }
        public bool IsDeleted { get; protected set; } = false;
        public DateTime? DeletedAt { get; protected set; }

        public void SetUpdatedAt() => UpdatedAt = DateTime.UtcNow;
        public void SoftDelete()
        {
            IsDeleted = true;
            DeletedAt = DateTime.Now;
        }

        public void Restore()
        {
            IsDeleted = false;
            DeletedAt = null;
            UpdatedAt = DateTime.Now;
        }
    }
}
