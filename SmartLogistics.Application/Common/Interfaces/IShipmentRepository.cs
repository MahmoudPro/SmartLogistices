using SmartLogistics.Domain.Entities;

namespace SmartLogistics.Application.Common.Interfaces
{
    public interface IShipmentRepository
    {
        Task<Shipment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Shipment?> GetByTrackingNumberAsync(string trackingNumber, CancellationToken cancellationToken = default);
        Task AddAsync(Shipment shipment, CancellationToken cancellationToken = default);
        void Update(Shipment shipment); // الـ Update في EF Core لا يحتاج Async لأنه يغير الـ State في الـ Memory فقط
    }
}
