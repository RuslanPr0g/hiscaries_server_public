using System.Collections.Generic;

namespace Enterprise.Domain;

public interface IAggregateRoot
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void ClearEvents();
}