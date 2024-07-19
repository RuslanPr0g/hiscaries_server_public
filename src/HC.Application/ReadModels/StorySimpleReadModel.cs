using HC.Domain.Stories;
using System;

public sealed class StorySimpleReadModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string AuthorName { get; set; }
    public int AgeLimit { get; set; }
    public byte[] ImagePreview { get; set; }
    public DateTime DatePublished { get; set; }
    public DateTime DateWritten { get; set; }
    public string Publisher { get; set; }

    public static StorySimpleReadModel FromDomainModel(Story story)
    {
        throw new NotImplementedException();
    }
}