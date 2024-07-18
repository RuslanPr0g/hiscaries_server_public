using System.Collections.Generic;

public class StoryPagesCreateRequest
{
    public string StoryId { get; set; }

    public IEnumerable<string> Content { get; set; }
}