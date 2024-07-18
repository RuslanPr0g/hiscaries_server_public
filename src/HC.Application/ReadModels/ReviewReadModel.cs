public sealed class ReviewReadModel
{
    public UserSimpleReadModel Publisher { get; init; }
    public UserSimpleReadModel Reviewer { get; init; }
    public string Message { get; init; }
    public string Username { get; init; }
}