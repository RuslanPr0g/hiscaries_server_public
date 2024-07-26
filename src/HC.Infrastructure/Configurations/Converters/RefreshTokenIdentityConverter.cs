namespace HC.Infrastructure.Configurations.Converters;

public class RefreshTokenIdentityConverter : IdentityConverter<RefreshTokenId>
{
    public RefreshTokenIdentityConverter() :
        base((x) => new RefreshTokenId(x))
    {
    }
}
