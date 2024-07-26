namespace HC.Infrastructure.Configurations.Converters;

internal class RefreshTokenIdentityConverter : IdentityConverter<RefreshTokenId>
{
    internal RefreshTokenIdentityConverter() :
        base((x) => new RefreshTokenId(x))
    {
    }
}
