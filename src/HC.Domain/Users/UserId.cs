using System;

namespace HC.Domain.Users;

public sealed record UserId(Guid Value) : Identity(Value)
{
    public static implicit operator UserId(Guid identity) => new(identity);
    public static implicit operator Guid(UserId identity) => identity.Value;
}
