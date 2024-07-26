using HC.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;

public sealed class UserReadModel
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public DateTime AccountCreated { get; set; }
    public DateTime BirthDate { get; set; }
    public int Age { get; set; }
    public bool Banned { get; set; }
    public string Role { get; set; }
    public int TotalStories { get; set; }
    public int TotalReads { get; set; }
    public double AverageScore { get; set; }
    public IEnumerable<StoryBookMarkReadModel> BookmarkedStories { get; set; }
    public IEnumerable<ReviewReadModel> Reviews { get; set; }
    public UserReadHistoryReadModel ReadHistory { get; set; }

    public static UserReadModel FromDomainModel(User user, int totalStories, int totalReads, double averageScore)
    {
        return new UserReadModel
        {
            Id = user.Id,
            Username = user.Username,
            AccountCreated = user.AccountCreated,
            BirthDate = user.BirthDate,
            Age = DateTime.UtcNow.Year - user.BirthDate.Year,
            Banned = user.Banned,
            Role = user.Role.ToString(),
            TotalStories = totalStories,
            TotalReads = totalReads,
            AverageScore = averageScore,
            BookmarkedStories = user.BookMarks.Select(StoryBookMarkReadModel.FromDomainModel),
            Reviews = user.Reviews.Select(x =>
                ReviewReadModel.FromDomainModel(x,
                // TODO: how the hell am I supposed to fill in info for both of them?
                UserSimpleReadModel.FromDomainModel(user, totalStories, totalReads, averageScore),
                UserSimpleReadModel.FromDomainModel(user, totalStories, totalReads, averageScore))),
            ReadHistory = UserReadHistoryReadModel.FromDomainModel(user.ReadHistory),
        };
    }

    public static UserReadModel FromDomainModel(User user)
    {
        return new UserReadModel
        {
            Id = user.Id,
            Username = user.Username,
            AccountCreated = user.AccountCreated,
            BirthDate = user.BirthDate,
            Age = DateTime.UtcNow.Year - user.BirthDate.Year,
            Banned = user.Banned,
            Role = user.Role.ToString(),
            BookmarkedStories = user.BookMarks.Select(StoryBookMarkReadModel.FromDomainModel),
            Reviews = user.Reviews.Select(x =>
                ReviewReadModel.FromDomainModel(x,
                UserSimpleReadModel.FromDomainModel(user),
                UserSimpleReadModel.FromDomainModel(user))),
            ReadHistory = UserReadHistoryReadModel.FromDomainModel(user.ReadHistory),
        };
    }
}
