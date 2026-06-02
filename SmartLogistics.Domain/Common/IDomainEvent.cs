using MediatR;

namespace SmartLogistics.Domain.Common
{
    public interface IDomainEvent: INotification
    {
        DateTime OccurredOn { get; }
    }
}
