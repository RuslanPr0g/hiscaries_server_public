using System;
using System.Collections.Generic;

public class StoryPagesCreateRequest
{
    public Guid StoryId { get; set; }

    public IEnumerable<string> Content { get; set; }
}