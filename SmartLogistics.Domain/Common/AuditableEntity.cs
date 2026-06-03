
namespace SmartLogistics.Domain.Common
{
    public abstract class AuditableEntity: BaseEntity
    {
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public string CreatedBy { get; private set; } = string.Empty;
        public DateTime? LastModifiedAt { get; private set; }
        public string LastModifiedBy { get; private set; } = string.Empty;
        public bool IsDeleted { get; private set; }
        public DateTime? DeletedAt { get; private set; }
        public string DeletedBy { get; private set; } = string.Empty;
        public void SoftDelete(string deletedBy)
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            DeletedBy = deletedBy;
        }
        public void UndoDelete()
        {
            IsDeleted = false;
            DeletedAt = null;
            DeletedBy = string.Empty;
        }
    }
}
