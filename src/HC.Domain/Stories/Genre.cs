namespace HC.Domain.Stories;

public sealed class Genre : Entity<GenreId>
{
    private Genre(
        GenreId id,
        string name,
        string description,
        byte[] imagePreview) : base(id)
    {
        Name = name;
        Description = description;
        ImagePreview = imagePreview;
    }

    public static Genre Create(
        GenreId id,
        string name,
        string description,
        byte[] imagePreview) =>
        new Genre(id, name, description, imagePreview);

    public string Name { get; private set; }
    public string Description { get; private set; }
    public byte[] ImagePreview { get; private set; }

    public void UpdateInformation(string name, string description, byte[] imagePreview)
    {
        Name = name;
        Description = description;
        ImagePreview = imagePreview;
    }

    protected Genre()
    {
    }
}