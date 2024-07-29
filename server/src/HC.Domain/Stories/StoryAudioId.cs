using System;

namespace HC.Domain.Stories;

public record StoryAudioId(Guid Value) : Identity(Value)
{
    public static implicit operator StoryAudioId(Guid identity) => new(identity);
    public static implicit operator Guid(StoryAudioId identity) => identity.Value;
}
