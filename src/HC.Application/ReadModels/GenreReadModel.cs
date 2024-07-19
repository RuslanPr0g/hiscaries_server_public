using HC.Domain.Stories;

public sealed class GenreReadModel
{
    public string Name { get; init; }
    public string Description { get; init; }
    public byte[] ImagePreview { get; init; }

    public static GenreReadModel FromDomainModel(Genre genre)
    {
        return new GenreReadModel
        {
            Name = genre.Name,
            Description = genre.Description,
            ImagePreview = genre.ImagePreview,
        };
    }
}