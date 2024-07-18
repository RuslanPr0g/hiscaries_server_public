namespace HC.API.Requests;

public class CreateCommentRequest
{
    public int Id { get; set; }
    public string StoryId { get; set; }
    public string Content { get; set; }
}