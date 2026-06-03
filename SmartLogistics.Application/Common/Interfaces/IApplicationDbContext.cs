using SmartLogistics.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace SmartLogistics.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Shipment> Shipments { get; }
        DbSet<Driver> Drivers { get; }
        DbSet<AuditLog> AuditLogs { get; }
        DbSet<OutboxMessage> OutboxMessages { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    }
}
