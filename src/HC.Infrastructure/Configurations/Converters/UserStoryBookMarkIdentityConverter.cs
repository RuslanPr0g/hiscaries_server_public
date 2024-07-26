namespace HC.Infrastructure.Configurations.Converters;

internal class UserStoryBookMarkIdentityConverter : IdentityConverter<UserStoryBookMarkId>
{
    internal UserStoryBookMarkIdentityConverter() :
        base((x) => new UserStoryBookMarkId(x))
    {
    }
}