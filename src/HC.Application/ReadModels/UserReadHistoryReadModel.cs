using System.Collections.Generic;

public sealed class UserReadHistoryReadModel
{
    public IEnumerable<StoryWithProgressReadModel> Stories { get; set; }
}