using FluentValidation;

namespace SmartLogistics.Application.Shipments.Commands.CreateShipment
{
    public class CreateShipmentCommandValidator : AbstractValidator<CreateShipmentCommand>
    {
        public CreateShipmentCommandValidator()
        {
            RuleFor(c => c.Origin)
                .NotEmpty().WithMessage("مكان القيام (Origin) مطلوب ولا يمكن تركه فارغاً.")
                .MaximumLength(250).WithMessage("مكان القيام لا يمكن أن يتخطى 250 حرفاً.");

            RuleFor(c => c.Destination)
                .NotEmpty().WithMessage("وجهة الشحنة (Destination) مطلوبة.")
                .MaximumLength(250).WithMessage("وجهة الشحنة لا يمكن أن تتخطى 250 حرفاً.")
                .NotEqual(c => c.Origin).WithMessage("لا يمكن أن تكون وجهة الشحنة هي نفسها مكان القيام!");

            RuleFor(c => c.Weight)
                .GreaterThan(0).WithMessage("وزن الشحنة يجب أن يكون أكبر من صفر.")
                .LessThanOrEqualTo(50000).WithMessage("الحد الأقصى لوزن الشحنة الواحدة هو 50,000 كجم.");

            RuleFor(c => c.ShipmentDate)
                .NotEmpty().WithMessage("تاريخ الشحنة مطلوب.")
                .Must(BeAValidPastOrFutureDate).WithMessage("تاريخ الشحنة لا يمكن أن يكون قديماً جداً، يجب أن يكون من اليوم فصاعداً");
        }

        private bool BeAValidPastOrFutureDate(DateTime date)
        {
            var today = DateTime.Today;
            return date.Date >= today; // يسمح بالتاريخ الحالي أو المستقبل فقط

        }
    }
}