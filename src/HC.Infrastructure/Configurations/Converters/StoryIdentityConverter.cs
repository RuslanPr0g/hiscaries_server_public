using HC.Domain.Stories;

namespace HC.Infrastructure.Configurations.Converters;

internal class StoryIdentityConverter : IdentityConverter<StoryId>
{
    internal StoryIdentityConverter() :
        base((x) => new StoryId(x))
    {
    }
}
