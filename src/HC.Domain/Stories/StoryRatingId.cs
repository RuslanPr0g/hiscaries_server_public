using System;

namespace HC.Domain.Stories;

public sealed record StoryRatingId(Guid Value) : Identity(Value)
{
    public static implicit operator StoryRatingId(Guid identity) => new(identity);
    public static implicit operator Guid(StoryRatingId identity) => identity.Value;
}
