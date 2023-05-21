using MediatR;

namespace NerdStore.Core.Messages.CommonMessages.DomainEvents;

public class DomainEvent : Message, INotification
{
    public DomainEvent(Guid aggregateId)
    {
        AggregateId = aggregateId;
    }
}