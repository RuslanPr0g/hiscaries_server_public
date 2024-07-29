namespace HC.API.Requests;

public class StoryPageReadRequest
{
    public int Id { get; set; }

    public int StoryId { get; set; }

    public int Page { get; set; }

    public string Content { get; set; }
}