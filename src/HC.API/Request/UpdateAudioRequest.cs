using System;

namespace HC.API.Requests;

public sealed class UpdateAudioRequest
{
    public Guid StoryId { get; set; }
    public string Name { get; set; }
    public byte[] Audio { get; set; }
}