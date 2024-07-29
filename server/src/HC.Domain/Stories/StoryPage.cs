namespace HC.Domain.Stories;

public sealed class StoryPage
{
    public StoryPage(
        StoryId storyId,
        int page,
        string content)
    {
        StoryId = storyId;
        Page = page;
        Content = content;
    }

    // TODO: THESE TWO ARE PRIMARY KEYS
    public StoryId StoryId { get; init; }
    public int Page { get; init; }

    public string Content { get; init; }

    private StoryPage()
    {
    }
}