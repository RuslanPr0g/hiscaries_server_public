using System;

namespace HC.API.Requests;

public class CreateCommentRequest
{
    public Guid Id { get; set; }
    public Guid StoryId { get; set; }
    public string Content { get; set; }
}