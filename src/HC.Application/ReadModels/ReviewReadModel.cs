using HC.Domain.Users;
using System;

public sealed class ReviewReadModel
{
    public Guid Id { get; set; }
    public UserSimpleReadModel Publisher { get; init; }
    public UserSimpleReadModel Reviewer { get; init; }
    public string Message { get; init; }
    public string Username { get; init; }

    public static ReviewReadModel FromDomainModel(Review review, User publisher, User reviewer)
    {
        return new ReviewReadModel
        {
            Id = review.Id,
            Publisher = UserSimpleReadModel.FromDomainModel(publisher),
            Reviewer = UserSimpleReadModel.FromDomainModel(reviewer),
            Message = review.Message,
            Username = review.Username,
        };
    }
}