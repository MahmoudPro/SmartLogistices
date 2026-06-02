using FluentValidation;
using MediatR;

namespace SmartLogistics.Application.Common.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    // حقن كافة الـ Validators المسجلة في السيستم تلقائياً
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            // تشغيل الفحص على الطلب القادم
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            // تجميع الأخطاء لو وجدت
            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            // إذا كان هناك أي خطأ، ارمي Exception مخصص للأخطاء فوراً ومنع الدخول للـ Handler
            if (failures.Count != 0)
            {
                throw new ValidationException(failures);
            }
        }

        // إذا كانت البيانات سليمة، انتقل للخطوة التالية (الـ Handler الفعلي)
        return await next();
    }
}