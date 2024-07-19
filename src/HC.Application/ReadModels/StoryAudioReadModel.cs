using System;

public class StoryAudioReadModel
{
    public int Id { get; set; }
    public Guid FileId { get; set;}
    public DateTime DateAdded { get; set;}
    public string Name { get; set;}
    public byte[] Bytes { get; set;}
}