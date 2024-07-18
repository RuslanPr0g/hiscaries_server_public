namespace HC.API.Requests;

public sealed class CreateAudioModelRequest
{
    public string Name { get; set; }
    public byte[] Audio { get; set; }
}