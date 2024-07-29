using HC.Domain.Stories;
using System;

public sealed class GenreReadModel
{
    public Guid Id { get; set; }
    public string Name { get; init; }
    public string Description { get; init; }
    public byte[] ImagePreview { get; init; }

    public static GenreReadModel FromDomainModel(Genre genre)
    {
        return new GenreReadModel
        {
            Id = genre.Id,
            Name = genre.Name,
            Description = genre.Description,
            ImagePreview = genre.ImagePreview,
        };
    }
}