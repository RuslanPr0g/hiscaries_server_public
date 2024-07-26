using HC.Domain.Stories;

namespace HC.Infrastructure.Configurations.Converters;

public class StoryRatingIdentityConverter : IdentityConverter<StoryRatingId>
{
    public StoryRatingIdentityConverter() :
        base((x) => new StoryRatingId(x))
    {
    }
}
