using HC.Domain.Stories;
using System;

namespace HC.Domain.Users;

public sealed class UserStoryBookMark : Entity<UserStoryBookMarkId>
{
    public UserStoryBookMark(
        UserStoryBookMarkId id,
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

    private UserStoryBookMark()
    {
    }
}