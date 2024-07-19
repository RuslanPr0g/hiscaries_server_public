using HC.Domain.Stories;
using System;

namespace HC.Domain.Users;

public sealed class UserReadHistory : Entity<UserReadHistoryId>
{
    public UserReadHistory(
        UserReadHistoryId id,
        UserId user,
        StoryId story,
        DateTime dateRead,
        int pageRead) : base(id)
    {
        Id = id;
        UserId = user;
        StoryId = story;
        DateLastRead = dateRead;
        LastPageRead = pageRead;

        SoftDeleted = false;
    }

    public UserId UserId { get; init; }
    public StoryId StoryId { get; init; }
    public Story Story { get; }
    public DateTime DateLastRead { get; init; }
    public int LastPageRead { get; private set; }
    public bool SoftDeleted { get; init; }

    public decimal CalculatePercentageRead()
    {
        int totalStoryPage = Story.StoryPages.Count;
        int currentLastPage = LastPageRead;

        return (currentLastPage / totalStoryPage) * 100;
    }

    internal void ReadPage(int page)
    {
        LastPageRead = page;
    }

    private UserReadHistory()
    {
    }
}