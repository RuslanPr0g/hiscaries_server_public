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
        DateTime birthDate,
        bool banned,
        RefreshTokenId refreshTokenId) : base(id)
    {
        Username = username;
        Email = email;
        Password = password;
        AccountCreated = accountCreated;
        BirthDate = birthDate;
        Banned = banned;
        RefreshTokenId = refreshTokenId;

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
    public RefreshToken RefreshToken { get; init; }

    public ICollection<Review> Reviews { get; }

    // TODO: "Your cannot review your profile!"

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

    private User()
    {
    }
}