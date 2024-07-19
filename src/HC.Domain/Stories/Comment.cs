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
        UpdatedAt = commentedAt;
        Score = score;
    }

    public static Comment Create(
        CommentId id,
        StoryId story,
        UserId user,
        string content,
        DateTime commentedAt,
        int score) => new Comment(id, story, user, content, commentedAt, score);

    internal void UpdateContent(string content, int score, DateTime updatedAt)
    {
        Content = content;
        Score = score;
        UpdatedAt = updatedAt;
    }

    public StoryId StoryId { get; init; }
    public Story Story { get; init; }
    public UserId UserId { get; init; }
    public User User { get; init; }

    public string Content { get; private set; }
    public int Score { get; private set; }
    public DateTime CommentedAt { get; init; }
    public DateTime UpdatedAt { get; private set; }

    protected Comment()
    {
    }
}