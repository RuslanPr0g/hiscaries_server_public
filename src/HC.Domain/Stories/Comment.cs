using HC.Domain.Users;
using System;

namespace HC.Domain.Stories;

public class Comment : Entity<CommentId>
{
    private Comment(
        CommentId id,
        StoryId story,
        UserId user,
        string content,
        DateTime commentedAt,
        int score) : base(id)
    {
        StoryId = story;
        UserId = user;
        Content = content;
        CommentedAt = commentedAt;
        Score = score;
    }

    public static Comment Create(
        CommentId id,
        StoryId story,
        UserId user,
        string content,
        DateTime commentedAt,
        int score) => new Comment(id, story, user, content, commentedAt, score);

    public StoryId StoryId { get; init; }
    public Story Story { get; init; }
    public UserId UserId { get; init; }
    public User User { get; init; }

    public string Content { get; init; }
    public int Score { get; init; }
    public DateTime CommentedAt { get; init; }

    protected Comment()
    {
    }
}