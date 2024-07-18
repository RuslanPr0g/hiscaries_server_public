public sealed class UpdateAudioRequest
{
    public int AudioId { get; set; }
    public byte[] Audio { get; set; }
    public string Name { get; set; }
}