using HC.Domain.Stories;

namespace HC.Infrastructure.Configurations.Converters;

public class StoryIdentityConverter : IdentityConverter<StoryId>
{
    public StoryIdentityConverter() :
        base((x) => new StoryId(x))
    {
    }
}
