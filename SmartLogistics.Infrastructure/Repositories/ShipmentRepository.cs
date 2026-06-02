using SmartLogistics.Application.Common.Interfaces;
using SmartLogistics.Domain.Entities;
using SmartLogistics.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace SmartLogistics.Infrastructure.Repositories
{
    public class ShipmentRepository : IShipmentRepository
    {
        private readonly ApplicationDbContext _context;
        public ShipmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Shipment shipment, CancellationToken cancellationToken = default)
        {
            await _context.Shipments.AddAsync(shipment, cancellationToken);
        }

        public async Task<Shipment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Shipments.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<Shipment?> GetByTrackingNumberAsync(string trackingNumber, CancellationToken cancellationToken = default)
        {
            return await _context.Shipments.FirstOrDefaultAsync(s => s.TrackingNumber == trackingNumber, cancellationToken);
        }

        public void Update(Shipment shipment)
        {
            _context.Shipments.Update(shipment);
        }
    }
}
