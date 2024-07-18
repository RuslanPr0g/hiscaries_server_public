using System;
using System.Collections.Generic;

namespace HC.API.Requests;

public class StoryCreateRequest
{
    public string StoryId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string AuthorName { get; set; }

    public IEnumerable<int> GenreIds { get; set; }

    public int AgeLimit { get; set; }

    public DateTime DateWritten { get; set; }

    public string ImagePreview { get; set; }
}