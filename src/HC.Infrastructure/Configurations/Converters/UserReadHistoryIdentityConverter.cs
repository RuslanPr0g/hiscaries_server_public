using HC.Domain.Users;

namespace HC.Infrastructure.Configurations.Converters;

internal class UserReadHistoryIdentityConverter : IdentityConverter<UserReadHistoryId>
{
    internal UserReadHistoryIdentityConverter() :
        base((x) => new UserReadHistoryId(x))
    {
    }
}
