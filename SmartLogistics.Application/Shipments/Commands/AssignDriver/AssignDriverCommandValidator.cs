using FluentValidation;

namespace SmartLogistics.Application.Shipments.Commands.AssignDriver
{
    public class AssignDriverCommandValidator: AbstractValidator<AssignDriverCommand>
    {
        public AssignDriverCommandValidator()
        {
            RuleFor(c => c.ShipmentId)
                .NotEmpty().WithMessage("معرف الشحنة (ShipmentId) مطلوب.");
            
            RuleFor(c => c.DriverId)
                .NotEmpty().WithMessage("معرف السائق (DriverId) مطلوب.");
        }
    }
}
