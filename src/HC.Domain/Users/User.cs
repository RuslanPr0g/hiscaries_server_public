using Enterprise.Domain;
using HC.Domain.Stories;
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

    public string Username { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public DateTime AccountCreated { get; init; }
    public DateTime BirthDate { get; private set; }
    public bool Banned { get; init; }

    public UserRole Role { get; private set; }

    public RefreshTokenId RefreshTokenId { get; init; }
    public RefreshToken RefreshToken { get; private set; }

    public ICollection<Review> Reviews { get; }
    public ICollection<UserReadHistory> ReadHistory { get; }

    public void ReadStoryPage(StoryId storyId, int page, DateTime readDate, UserReadHistoryId generatedHistoryPageId)
    {
        var historyItem = ReadHistory.FirstOrDefault(x => x.StoryId == storyId);

        if (historyItem is not null)
        {
            historyItem.ReadPage(page);
        }
        else
        {
            ReadHistory.Add(new UserReadHistory(generatedHistoryPageId, Id, storyId, readDate, page));
        }
    }

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

    public bool ValidateRefreshToken(string jwtId)
    {
        if (RefreshToken is null || RefreshToken.JwtId != jwtId)
        {
            return false;
        }

        return RefreshToken.Validate();
    }

    public void UpdatePersonalInformation(
        string? updatedUsername,
        string? email,
        DateTime? birthDate)
    {
        if (!string.IsNullOrEmpty(updatedUsername))
        {
            Username = updatedUsername;
        }

        if (!string.IsNullOrEmpty(email))
        {
            Email = email;
        }

        if (birthDate.HasValue)
        {
            BirthDate = birthDate.Value;
        }
    }

    public void UpdatePassword(string password)
    {
        Password = password;
    }

    private User()
    {
    }
}