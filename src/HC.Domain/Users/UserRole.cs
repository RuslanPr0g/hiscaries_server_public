using System;
using static HC.Domain.Users.UserRole;

namespace HC.Domain.Users;

public sealed record class UserRole : IValueObject<UserRoleEnum>
{
    public UserRole(UserRoleEnum role)
    {
        if (!Enum.IsDefined(role))
        {
            throw new ArgumentException($"Role is invalid. The provided value is {role}");
        }

        Value = role;
    }

    public UserRoleEnum Value { get; init; }

    public bool IsAdmin => Value == UserRoleEnum.Admin;
    public bool IsReader => Value == UserRoleEnum.Admin;
    public bool IsPublisher => Value == UserRoleEnum.Admin;

    private UserRole()
    {
    }

    public enum UserRoleEnum
    {
        Reader,
        Publisher,
        Admin
    }
}