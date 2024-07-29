using System;

namespace HC.Domain;
public record Identity(Guid Value)
{
    public static implicit operator Identity(Guid identity) => new(identity);
    public static implicit operator Guid(Identity identity) => identity.Value;
}
