using HC.Domain.Users;
using System;
using System.Collections.Generic;

public sealed class UserReadModel
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public DateTime AccountCreated { get; set; }
    public string BirthDate { get; set; }
    public int Age { get; set; }
    public bool Banned { get; set; }
    public string Role { get; set; }
    public int TotalStories { get; set; }
    public int TotalReads { get; set; }
    public double AverageScore { get; set; }
    public IEnumerable<StoryBookMarkReadModel> BookmarkedStories { get; set; }
    public IEnumerable<ReviewReadModel> Reviews { get; set; }
    public UserReadHistoryReadModel ReadHistory { get; set; }

    public static UserReadModel FromDomainModel(User user)
    {
        throw new NotImplementedException();
    }
}
