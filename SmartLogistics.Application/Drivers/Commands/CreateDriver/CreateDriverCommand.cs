using MediatR;
using SmartLogistics.Application.Common.Interfaces;
using SmartLogistics.Domain.Entities;

namespace SmartLogistics.Application.Drivers.Commands.CreateDriver
{
    public sealed record CreateDriverCommand(
        string FirstName,
        string LastName,
        string LicenseNumber,
        DateTime LicenseExpirationDate,
        string? Nationality,
        string? NationalID,
        string? PassportNumber

        ) : IRequest<Guid>;

    public class CreateDriverCommandHandler : IRequestHandler<CreateDriverCommand, Guid>
    {
        private readonly IDriverRepository _driverRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateDriverCommandHandler(IDriverRepository driverRepository, IUnitOfWork unitOfWork)
        {
            _driverRepository = driverRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(CreateDriverCommand request, CancellationToken cancellationToken)
        {
            var driver = Driver.Create(
                request.FirstName,
                request.LastName,
                request.LicenseNumber,
                request.LicenseExpirationDate,
                request.Nationality,
                request.NationalID,
                request.PassportNumber
                );

            await _driverRepository.AddAsync(driver, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return driver.Id;
        }
    }
}
