using FluentValidation;

namespace SmartLogistics.Application.Drivers.Commands.CreateDriver
{
    public class CreateDriverCommandValidator: AbstractValidator<CreateDriverCommand>
    {
        public CreateDriverCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");
            RuleFor(x => x.LicenseNumber)
                .NotEmpty().WithMessage("License number is required.")
                .MaximumLength(20).WithMessage("License number cannot exceed 20 characters.");
            RuleFor(x => x.LicenseExpirationDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("License expiration date must be in the future.");
            RuleFor(x => x.Nationality)
                .MaximumLength(50).WithMessage("Nationality cannot exceed 50 characters.");
        }
    }
}
