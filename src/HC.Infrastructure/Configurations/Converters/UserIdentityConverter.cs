using HC.Domain.Users;

namespace HC.Infrastructure.Configurations.Converters;

internal class UserIdentityConverter : IdentityConverter<UserId>
{
    internal UserIdentityConverter() :
        base((x) => new UserId(x))
    {
    }
}
