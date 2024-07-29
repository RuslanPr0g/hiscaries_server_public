using HC.Domain.Stories;
using System;
using System.Collections.Generic;
using System.Linq;

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

    public IEnumerable<StoryAudioReadModel> Audios { get; set; }

    public static StoryReadModel FromDomainModel(Story story, int totalReadCount)
    {
        return new StoryReadModel
        {
            Id = story.Id,
            Title = story.Title,
            Description = story.Description,
            AuthorName = story.AuthorName,
            AgeLimit = story.AgeLimit,
            ImagePreview = story.ImagePreview,
            DatePublished = story.DatePublished,
            DateWritten = story.DateWritten,
            Publisher = UserReadModel.FromDomainModel(story.Publisher),
            Genres = story.Genres.Select(GenreReadModel.FromDomainModel),
            Pages = story.Contents.Select(StoryPageReadModel.FromDomainModel),
            AverageScore = story.Comments.Average(x => x.Score),
            CommentCount = story.Comments.Count,
            ReadCount = totalReadCount,
            PageCount = story.Contents.Count
        };
    }
}