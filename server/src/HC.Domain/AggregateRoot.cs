using HC.Domain;
using System.Collections.Generic;

namespace Enterprise.Domain;

public abstract class AggregateRoot<T> : Entity<T>, IAggregateRoot
    where T : Identity
{
    private readonly ICollection<IDomainEvent> _domainEvents;

    protected AggregateRoot(T id) : base(id)
    {
        Id = id;
        _domainEvents = [];
    }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => [.. _domainEvents];

    public void ClearEvents()
    {
        _domainEvents.Clear();
    }

    public void PublishEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected AggregateRoot()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        _domainEvents = [];
    }
}