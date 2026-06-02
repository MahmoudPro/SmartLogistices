using SmartLogistics.Domain.Entities;

namespace SmartLogistics.Application.Common.Interfaces
{
    public interface IDriverRepository
    {
        Task<Driver?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Driver>> GetAvailableDriversAsync(CancellationToken cancellationToken = default);
        Task AddAsync(Driver driver, CancellationToken cancellationToken = default);
        //Task UpdateAsync(Driver driver, CancellationToken cancellationToken = default);
        //Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
