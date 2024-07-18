using System;

public sealed class StoryPageReadModel
{
    public Guid StoryId { get; set; }
    public int Page { get; init; }
    public string Content { get; init; }
}