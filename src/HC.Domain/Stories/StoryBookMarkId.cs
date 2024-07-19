using HC.Domain;
using HC.Domain.Stories;
using System;

public sealed record StoryBookMarkId(Guid Value) : Identity(Value)
{
    public static implicit operator StoryBookMarkId(Guid identity) => new(identity);
    public static implicit operator Guid(StoryBookMarkId identity) => identity.Value;
}