using HC.Domain.Stories;

namespace HC.Infrastructure.Configurations.Converters;

internal class StoryRatingIdentityConverter : IdentityConverter<StoryRatingId>
{
    internal StoryRatingIdentityConverter() :
        base((x) => new StoryRatingId(x))
    {
    }
}
