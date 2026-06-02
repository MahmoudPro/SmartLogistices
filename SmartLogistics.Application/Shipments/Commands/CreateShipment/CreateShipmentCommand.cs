using MediatR;
using SmartLogistics.Domain.Entities;
using SmartLogistics.Application.Common.Interfaces;


namespace SmartLogistics.Application.Shipments.Commands.CreateShipment
{
    public sealed record CreateShipmentCommand(
        string Origin,
        string Destination,
        DateTime ShipmentDate,
        decimal Weight,
        string Description) : IRequest<Guid>;


    public class CreateShipmentCommandHandler : IRequestHandler<CreateShipmentCommand, Guid>
    {
        private readonly IShipmentRepository _shipmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateShipmentCommandHandler(IShipmentRepository shipmentRepository, IUnitOfWork unitOfWork)
        {
            _shipmentRepository = shipmentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateShipmentCommand request, CancellationToken cancellationToken)
        {
            var shipment = Shipment.Create(
                request.Origin,
                request.Destination,
                request.Weight,
                request.ShipmentDate,
                Guid.NewGuid()
                );
            await _shipmentRepository.AddAsync(shipment);
            await _unitOfWork.SaveChangesAsync();
            return shipment.Id;
        }
    }
}
