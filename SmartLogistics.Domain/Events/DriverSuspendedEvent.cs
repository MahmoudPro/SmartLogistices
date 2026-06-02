using SmartLogistics.Domain.Common;

namespace SmartLogistics.Domain.Events
{
    public class DriverSuspendedEvent : IDomainEvent
    {
        public Guid DriverId { get; }
        public string Reason { get; }
        public DateTime OccurredOn { get; } = DateTime.UtcNow;

        public DriverSuspendedEvent(Guid driverId, string reason)
        {
            DriverId = driverId;
            Reason = reason;
        }
    }
}
