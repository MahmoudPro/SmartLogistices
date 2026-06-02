
namespace SmartLogistics.Domain.Entities
{
    public sealed class AuditLog
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string? UserId { get; private set; } 
        public string? Action { get; private set; } // Create, Update, Delete
        public string TableName { get; private set; } 
        public DateTime DateTime { get; private set; } = DateTime.UtcNow;
        public string? KeyValues { get; private set; }   // الـ ID الخاص بالسجل المنشأ/المعدل (JSON)
        public string? OldValues { get; private set; } // القيم قبل التعديل (JSON)
        public string? NewValues { get; private set; } // القيم بعد التعديل (JSON)

        // Constructor الخاص بـ EF Core
        private AuditLog() { }

        public static AuditLog Create(string userId, string action, string tableName, string keyValues, string? oldValues, string? newValues)
        {
            return new AuditLog
            {
                UserId = userId,
                Action = action,
                TableName = tableName,
                KeyValues = keyValues,
                OldValues = oldValues,
                NewValues = newValues
            };
        }
    }
}