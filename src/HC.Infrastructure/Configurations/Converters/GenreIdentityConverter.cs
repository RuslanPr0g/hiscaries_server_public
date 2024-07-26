using HC.Domain.Stories;

namespace HC.Infrastructure.Configurations.Converters;

internal class GenreIdentityConverter : IdentityConverter<GenreId>
{
    internal GenreIdentityConverter() :
        base((x) => new GenreId(x))
    {
    }
}
