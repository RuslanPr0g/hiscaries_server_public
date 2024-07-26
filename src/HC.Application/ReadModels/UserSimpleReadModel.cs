using HC.Domain.Users;
using System;

public sealed class UserSimpleReadModel
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public DateTime BirthDate { get; set; }
    public int Age { get; set; }
    public bool Banned { get; set; }
    public string Role { get; set; }
    public int TotalStories { get; set; }
    public int TotalReads { get; set; }
    public double AverageScore { get; set; }

    public static UserSimpleReadModel FromDomainModel(User user, int totalStories, int totalReads, double averageScore)
    {
        return new()
        {
            Id = user.Id,
            Username = user.Username,
            BirthDate = user.BirthDate,
            Age = DateTime.UtcNow.Year - user.BirthDate.Year,
            Banned = user.Banned,
            Role = user.Role.ToString(),
            TotalStories = totalStories,
            TotalReads = totalReads,
            AverageScore = averageScore
        };
    }

    public static UserSimpleReadModel FromDomainModel(User user)
    {
        return new()
        {
            Id = user.Id,
            Username = user.Username,
            BirthDate = user.BirthDate,
            Age = DateTime.UtcNow.Year - user.BirthDate.Year,
            Banned = user.Banned,
            Role = user.Role.ToString(),
        };
    }
}