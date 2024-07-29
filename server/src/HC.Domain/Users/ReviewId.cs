using HC.Domain;
using System;

public sealed record ReviewId(Guid Value) : Identity(Value)
{
    public static implicit operator ReviewId(Guid identity) => new(identity);
    public static implicit operator Guid(ReviewId identity) => identity.Value;
}
