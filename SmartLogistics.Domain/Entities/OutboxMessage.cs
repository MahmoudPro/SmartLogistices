using System;

namespace SmartLogistics.Domain.Entities
{
    public sealed class OutboxMessage
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string? Type { get; private set; }    // اسم كلاس الـ Event (e.g., ShipmentDeliveredEvent)
        public string? Content { get; private set; } // محتوى الـ Event مٌحول إلى JSON String
        public DateTime OccurredOnUtc { get; private set; }
        public DateTime? ProcessedOnUtc { get; private set; }       // متى قامت الـ Background Job بمعالجته؟
        public string? Error { get; private set; }                  // لو حدث خطأ أثناء المعالجة يسجل هنا

        private OutboxMessage() { }

        public static OutboxMessage Create(string type, string content, DateTime occurredOnUtc)
        {
            return new OutboxMessage
            {
                Type = type,
                Content = content,
                OccurredOnUtc = occurredOnUtc
            };
        }

        public void MarkAsProcessed()
        {
            ProcessedOnUtc = DateTime.UtcNow;
        }

        public void LogError(string error)
        {
            Error = error;
        }
    }
}