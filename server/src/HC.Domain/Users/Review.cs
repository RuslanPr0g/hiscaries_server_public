namespace HC.Domain.Users;

public sealed class Review : Entity<ReviewId>
{
    public Review(
        ReviewId id,
        UserId publisher,
        UserId reviewer,
        string message,
        string username) : base(id)
    {
        PublisherId = publisher;
        ReviewerId = reviewer;
        Message = message;
        Username = username;
    }

    public User Publisher { get; init; }
    public User Reviewer { get; init; }
    public UserId PublisherId { get; init; }
    public UserId ReviewerId { get; init; }
    public string Message { get; private set; }
    public string Username { get; init; }

    private Review()
    {
    }

    internal void Edit(string message)
    {
        if (!string.IsNullOrEmpty(message))
        {
            Message = message;
        }
    }
}