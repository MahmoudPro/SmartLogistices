using MediatR;
using SmartLogistics.Application.Common.Interfaces;

namespace SmartLogistics.Application.Shipments.Commands.AssignDriver
{
    public sealed record AssignDriverCommand(
        Guid ShipmentId,
        Guid DriverId) : IRequest<Unit>;

    public class AssignDriverCommandHandler : IRequestHandler<AssignDriverCommand, Unit>
    {
        private readonly IShipmentRepository _shipmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDriverRepository _driverRepository;

        public AssignDriverCommandHandler(IShipmentRepository shipmentRepository, 
            IUnitOfWork unitOfWork,
            IDriverRepository driverRepository
            )
        {
            _shipmentRepository = shipmentRepository;
            _unitOfWork = unitOfWork;
            _driverRepository = driverRepository;
        }
        public async Task<Unit> Handle(AssignDriverCommand request, CancellationToken cancellationToken)
        {
           var shipment = await _shipmentRepository.GetByIdAsync(request.ShipmentId, cancellationToken);
            if (shipment is null)
            {
                throw new KeyNotFoundException($"Shipment with ID {request.ShipmentId} not found.");
            }

            var driver = await _driverRepository.GetByIdAsync(request.DriverId, cancellationToken);
            
            if (driver is null)
                throw new KeyNotFoundException($"السائق ذو المعرف {request.DriverId} غير موجود.");

            // تحقق من صلاحية رخصة السائق من خلال منطق الـ Domain الذي كتبته بنفسك سابقاً!
            if (!driver.IsLicenseValid())
                throw new InvalidOperationException("لا يمكن تعيين السائق لأن رخصته منتهية الصلاحية.");

            shipment.AssignDriver(driver.Id);

            driver.MakeUnavailable();// اجعل السائق غير متاح بعد تعيينه لشحنة

            _shipmentRepository.Update(shipment);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}
