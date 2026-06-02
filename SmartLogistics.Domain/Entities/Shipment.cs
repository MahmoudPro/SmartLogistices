using SmartLogistics.Domain.Common;
using SmartLogistics.Domain.Enums;
using SmartLogistics.Domain.Events;

namespace SmartLogistics.Domain.Entities
{
    // ✅ الكود الصحيح — State Machine حقيقية
    public sealed class Shipment : AuditableEntity
    {
        public string TrackingNumber { get; private set; } = string.Empty;
        public string Origin { get; private set; } = string.Empty;
        public string Destination { get; private set; } = string.Empty;
        public string? Description { get; private set; }  // nullable
        public decimal Weight { get; private set; }        // decimal وليس double!
        public DateTime ScheduledDate { get; private set; }
        public DateTime? EstimatedDeliveryDate { get; private set; }
        public DateTime? ActualDeliveryDate { get; private set; }
        public ShipmentStatus Status { get; private set; } = ShipmentStatus.Submitted;
        public Guid? CustomerId { get; private set; }      // nullable!

        public Guid? DriverId { get; private set; }        // nullable — قد لا يُعيَّن بعد
        public Driver? Driver { get; private set; } 
        private Shipment() { }

        public static Shipment Create(
            string origin, string destination,
            decimal weight, DateTime scheduledDate, Guid customerId)
        {
            return new Shipment
            {
                TrackingNumber = GenerateTrackingNumber(),
                Origin = origin,
                Destination = destination,
                Weight = weight,
                ScheduledDate = scheduledDate,
                CustomerId = customerId,
                Status = ShipmentStatus.Submitted
            };
        }

        // ✅ كل transition لها method واضحة

        public void Confirm()
        {
            EnsureStatus(ShipmentStatus.Submitted, "Only submitted shipments can be confirmed.");
            Status = ShipmentStatus.Confirmed;
        }

        public void StartProcessing()
        {
            EnsureStatus(ShipmentStatus.Confirmed, "Only confirmed shipments can be processed.");
            Status = ShipmentStatus.Processing;
        }

        public void AssignDriver(Guid driverId)
        {
            EnsureStatus(ShipmentStatus.Processing, "Driver can only be assigned during processing.");
            DriverId = driverId;
            // لا نغير الـ Status هنا! هذه مسؤولية منفصلة
        }

        public void Dispatch()
        {
            if (Status != ShipmentStatus.Processing)
                throw new InvalidOperationException("Shipment must be in processing to dispatch.");
            if (DriverId is null)
                throw new InvalidOperationException("Cannot dispatch without a driver.");

            Status = ShipmentStatus.InTransit;
        }

        public void MarkOutForDelivery()
        {
            EnsureStatus(ShipmentStatus.InTransit, "Shipment must be in transit.");
            Status = ShipmentStatus.OutForDelivery;
        }

        public void MarkAsDelivered()
        {
            EnsureStatus(ShipmentStatus.OutForDelivery, "Shipment must be out for delivery.");
            Status = ShipmentStatus.Delivered;
            ActualDeliveryDate = DateTime.UtcNow;

            AddDomainEvent(new ShipmentDeliveredEvent(this.Id, DriverId, TrackingNumber, CustomerId));
        }

        public void Cancel(string reason)
        {
            if (Status == ShipmentStatus.Delivered || Status == ShipmentStatus.Returned)
                throw new InvalidOperationException(
                    "Cannot cancel a shipment that is already delivered or returned.");

            if (Status == ShipmentStatus.Cancelled)
                throw new InvalidOperationException("Shipment is already cancelled.");

            Status = ShipmentStatus.Cancelled;
        }

        public void MarkAsReturned()
        {
            if (Status != ShipmentStatus.InTransit && Status != ShipmentStatus.OutForDelivery)
                throw new InvalidOperationException(
                    "Shipment can only be returned while in transit or out for delivery.");

            Status = ShipmentStatus.Returned;
        }


        // ✅ Helper method لتجنب التكرار
        private void EnsureStatus(ShipmentStatus expected, string errorMessage)
        {
            if (Status != expected)
                throw new InvalidOperationException(errorMessage);
        }

        private static string GenerateTrackingNumber()
        {
            // Avoid C# 8.0 range operator and System.Index/Range types for compatibility
            var guid = Guid.NewGuid().ToString("N").ToUpper();
            var shortGuid = guid.Substring(0, 8);
            return $"SF-{DateTime.UtcNow:yyyyMMdd}-{shortGuid}";
        }
    }
}
