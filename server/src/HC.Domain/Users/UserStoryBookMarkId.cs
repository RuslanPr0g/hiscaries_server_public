using HC.Domain;
using System;

public sealed record UserStoryBookMarkId(Guid Value) : Identity(Value)
{
    public static implicit operator UserStoryBookMarkId(Guid identity) => new(identity);
    public static implicit operator Guid(UserStoryBookMarkId identity) => identity.Value;
}