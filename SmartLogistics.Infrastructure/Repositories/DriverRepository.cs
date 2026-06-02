using Microsoft.EntityFrameworkCore;
using SmartLogistics.Application.Common.Interfaces;
using SmartLogistics.Domain.Entities;
using SmartLogistics.Infrastructure.Data;

namespace SmartLogistics.Infrastructure.Repositories
{
    public class DriverRepository: IDriverRepository
    {
        private readonly ApplicationDbContext _context;
        public DriverRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Driver driver, CancellationToken cancellationToken = default)
        {
            await _context.Drivers.AddAsync(driver, cancellationToken);
        }
        public async Task<IEnumerable<Driver>> GetAvailableDriversAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Drivers.Where(d => d.IsAvailable).ToListAsync(cancellationToken);
        }
        public async Task<Driver?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Drivers.FindAsync(new object[] { id }, cancellationToken);
        }
    }
}
