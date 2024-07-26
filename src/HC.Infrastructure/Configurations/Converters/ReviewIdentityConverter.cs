namespace HC.Infrastructure.Configurations.Converters;

internal class ReviewIdentityConverter : IdentityConverter<ReviewId>
{
    internal ReviewIdentityConverter() :
        base((x) => new ReviewId(x))
    {
    }
}
