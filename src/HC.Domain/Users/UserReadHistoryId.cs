using System;

namespace HC.Domain.Users;

public sealed record UserReadHistoryId(Guid Value) : Identity(Value)
{
    public static implicit operator UserReadHistoryId(Guid identity) => new(identity);
    public static implicit operator Guid(UserReadHistoryId identity) => identity.Value;
}
