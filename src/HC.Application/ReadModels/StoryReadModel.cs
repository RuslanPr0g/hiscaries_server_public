using System;
using System.Collections.Generic;

public sealed class StoryReadModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string AuthorName { get; set; }
    public int AgeLimit { get; set; }
    public byte[] ImagePreview { get; set; }
    public byte[] Audio { get; set; }
    public DateTime DatePublished { get; set; }
    public DateTime DateWritten { get; set; }
    public UserReadModel Publisher { get; set; }
    public IEnumerable<GenreReadModel> Genres { get; set; }
    public IEnumerable<StoryPageReadModel> Pages { get; set; }
    public double AverageScore { get; set; }
    public int CommentCount { get; set; }
    public int ReadCount { get; set; }
    public int PageCount { get; set; }
}