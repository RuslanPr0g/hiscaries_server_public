using System;

namespace HC.Domain.Stories;

/// <summary>
/// Represents audio of a story, the actual data is on the disk,
/// this entity just stores reference to it in a way,
/// so that the id of the file is the same as the file name on the disk
/// </summary>
public sealed class StoryAudio : Entity<StoryAudioId>
{
    private StoryAudio(
        StoryAudioId id,
        DateTime dateAdded,
        string name) : base(id)
    {
        ArgumentNullException.ThrowIfNull(name);

        FileId = id.Value;
        CreatedAt = dateAdded;
        Name = name;
    }

    public static StoryAudio Create(
        StoryAudioId id,
        DateTime dateAdded,
        string name) =>
        new StoryAudio(id, dateAdded, name);

    internal void UpdateInformation(Guid fileId, string name, DateTime updatedAt)
    {
        FileId = fileId;
        Name = name;
        UpdatedAt = updatedAt;
    }

    public Guid FileId { get; private set; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; private set; }
    public string Name { get; private set; }

    protected StoryAudio()
    {
    }
}