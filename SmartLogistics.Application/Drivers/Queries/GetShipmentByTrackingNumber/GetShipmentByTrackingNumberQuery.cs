using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartLogistics.Application.Common.Interfaces;

namespace SmartLogistics.Application.Shipments.Queries.GetShipmentByTrackingNumber;

// 1. الـ Query Request
public sealed record GetShipmentByTrackingNumberQuery(string TrackingNumber) : IRequest<ShipmentDetailsDto?>;

// 2. الـ Handler
// ملاحظة هندسية: في الـ Queries يمكننا حقن الـ DbContext مباشرة للأداء العالي (Direct Read)، 
// أو استخدام الـ Repositories إذا كنت تفضل عزل الـ DbContext بالكامل. هنا سنستخدم الـ DbContext مباشرة للاستفادة من الـ Projection السريع.
public sealed class GetShipmentByTrackingNumberQueryHandler : IRequestHandler<GetShipmentByTrackingNumberQuery, ShipmentDetailsDto?>
{
    private readonly IApplicationDbContext _context;

    public GetShipmentByTrackingNumberQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ShipmentDetailsDto?> Handle(GetShipmentByTrackingNumberQuery request, CancellationToken cancellationToken)
    {
        return await _context.Shipments
            .AsNoTracking() // ⚡ سرعة أداء قصوى وتعطيل الـ Tracking
            .Include(s => s.Driver) // جلب بيانات السائق المرتبط بالشحنة
            .Where(s => s.TrackingNumber == request.TrackingNumber)
            .Select(s => new ShipmentDetailsDto(
                s.Id,
                s.TrackingNumber,
                s.Origin,
                s.Destination,
                s.Weight,
                s.Status.ToString(), // تحويل الـ Enum لـ string للعميل
                s.ScheduledDate,
                s.DriverId,
                s.Driver != null ? s.Driver.FirstName + " " + s.Driver.LastName : null
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }
}