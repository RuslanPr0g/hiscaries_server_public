using System;

namespace HC.API.Requests;

public class GetStoryFilesRequest
{
    public GetStoryFilesRequest(int id, Guid fileId, DateTime dateAdded, string name, byte[] bytes)
    {
        Id = id;
        FileId = fileId;
        CreatedAt = dateAdded;
        Name = name;
        Bytes = bytes;
    }

    public int Id { get; }
    public Guid FileId { get; }
    public DateTime CreatedAt { get; }
    public string Name { get; }
    public byte[] Bytes { get; }
}