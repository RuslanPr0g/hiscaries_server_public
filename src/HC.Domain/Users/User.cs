using Enterprise.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HC.Domain.Users;

public sealed class User : AggregateRoot<UserId>
{
    public User(
        UserId id,
        string username,
        string email,
        string password,
        DateTime accountCreated,
        DateTime birthDate) : base(id)
    {
        Username = username;
        Email = email;
        Password = password;
        AccountCreated = accountCreated;
        BirthDate = birthDate;

        Banned = false;
        Role = new UserRole(UserRole.UserRoleEnum.Reader);
    }

    public string Username { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public DateTime AccountCreated { get; init; }
    public DateTime BirthDate { get; init; }
    public bool Banned { get; init; }

    public UserRole Role { get; private set; }

    public RefreshTokenId RefreshTokenId { get; init; }
    public RefreshToken RefreshToken { get; private set; }

    public ICollection<Review> Reviews { get; }

    public void BecomePublisher()
    {
        if (Role.IsReader)
        {
            Role = new UserRole(UserRole.UserRoleEnum.Publisher);
        }
    }

    public void RemoveReview(ReviewId reviewId)
    {
        if (Reviews.Count > 0)
        {
            var review = Reviews.FirstOrDefault(x => x.Id == reviewId);

            if (review is not null)
            {
                Reviews.Remove(review);
            }
        }
    }

    public void PublishNewReview(UserId publisherId, string? message, ReviewId id)
    {
        RePublishReview(publisherId, message, id);
    }

    public void RePublishReview(UserId publisherId, string? message, ReviewId id)
    {
        if (!string.IsNullOrEmpty(message) && Reviews.Count > 0 && publisherId != Id)
        {
            var review = Reviews.FirstOrDefault(x => x.Id == id);

            if (review is null)
            {
                Reviews.Add(
                    new Review(
                        id, publisherId, Id, message, Username));
            }
            else
            {
                review.Edit(message);
            }
        }
    }

    public void UpdateRefreshToken(RefreshToken refreshToken)
    {
        if (RefreshToken is not null)
        {
            RefreshToken.Refresh(refreshToken);
        }
        else
        {
            RefreshToken = new RefreshToken(refreshToken);
        }
    }

    private User()
    {
    }
}