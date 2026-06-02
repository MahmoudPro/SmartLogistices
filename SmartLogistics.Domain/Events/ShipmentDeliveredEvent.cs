using System;
using SmartLogistics.Domain.Common;


namespace SmartLogistics.Domain.Events
{
    public sealed class ShipmentDeliveredEvent : IDomainEvent
    {
        public Guid ShipmentId { get; }
        public Guid? DriverId { get; }
        public string TrackingNumber { get; }   // مهم للإشعارات
        public Guid? CustomerId { get; }        // لإرسال البريد الإلكتروني
        public DateTime OccurredOn { get; }

        public ShipmentDeliveredEvent(
            Guid shipmentId,
            Guid? driverId,
            string trackingNumber,
            Guid? customerId)
        {
            ShipmentId = shipmentId;
            DriverId = driverId;
            TrackingNumber = trackingNumber;
            CustomerId = customerId;
            OccurredOn = DateTime.UtcNow;
        }
    }
}