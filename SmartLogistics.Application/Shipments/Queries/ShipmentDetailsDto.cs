
using SmartLogistics.Domain.Enums;

namespace SmartLogistics.Application.Shipments.Queries
{
    public sealed record ShipmentDetailsDto(
        Guid Id,
        string TrackingNumber,
        string Origin,
        string Destination,
        decimal Weight,
        string Status,
        DateTime ScheduledDate,
        Guid? DriverId,
        string? DriverName
    );
}
