using System;

namespace HC.API.Requests;

public class GetStoryFilesRequest
{
    public GetStoryFilesRequest(int id, Guid fileId, DateTime dateAdded, string name, byte[] bytes)
    {
        Id = id;
        FileId = fileId;
        DateAdded = dateAdded;
        Name = name;
        Bytes = bytes;
    }

    public int Id { get; }
    public Guid FileId { get; }
    public DateTime DateAdded { get; }
    public string Name { get; }
    public byte[] Bytes { get; }
}