using HC.Domain.Users;
using System;

namespace HC.Domain.Stories;

public sealed class StoryRating : Entity<StoryRatingId>
{
    public StoryRating(
        StoryRatingId id,
        StoryId story,
        UserId user,
        int score) : base(id)
    {
        Id = id;
        StoryId = story;
        UserId = user;

        if (score < 1 || score > 5)
        {
            // TODO: create a domain exception
            // TODO: create a value object for that
            throw new ArgumentException("Provided score was in invalid state.");
        }

        Score = score;
    }

    public StoryId StoryId { get; init; }
    public UserId UserId { get; init; }
    public User User { get; init; }
    public int Score { get; private set; }

    internal void UpdateScore(int score)
    {
        if (score < 1 || score > 5)
        {
            // TODO: create a domain exception
            // TODO: create a value object for that
            throw new ArgumentException("Provided score was in invalid state.");
        }

        Score = score;
    }

    private StoryRating()
    {
    }
}