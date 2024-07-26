using HC.Domain.Stories;
using System;

public class StoryAudioReadModel
{
    public Guid Id { get; set; }
    public Guid FileId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Name { get; set; }
    public byte[] Bytes { get; set; }

    public static StoryAudioReadModel FromDomainModel(StoryAudio audio, byte[] file)
    {
        return new()
        {
            Id = audio.Id,
            FileId = audio.FileId,
            CreatedAt = audio.CreatedAt,
            Name = audio.Name,
            Bytes = file
        };
    }
}