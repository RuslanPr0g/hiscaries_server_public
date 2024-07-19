using HC.Domain.Users;
using System;

namespace HC.Domain.Stories;

public sealed class StoryBookMark : Entity<StoryBookMarkId>
{
    public StoryBookMark(
        StoryBookMarkId id,
        UserId user,
        StoryId story,
        DateTime dateAdded) : base(id)
    {
        Id = id;
        UserId = user;
        StoryId = story;
        CreatedAt = dateAdded;
    }

    public UserId UserId { get; init; }
    public StoryId StoryId { get; init; }
    public DateTime CreatedAt { get; init; }

    private StoryBookMark()
    {
    }
}