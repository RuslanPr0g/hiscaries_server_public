using HC.Domain.Users;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using static HC.Domain.Users.UserRole;

namespace HC.Infrastructure.Configurations.Converters;

internal class UserRoleConverter : ValueConverter<UserRole, UserRoleEnum>
{
    public UserRoleConverter() : base(
        v => v.Value,
        v => new UserRole(v))
    {
    }
}
