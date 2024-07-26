using HC.Domain.Stories;

namespace HC.Infrastructure.Configurations.Converters;

public class StoryAudioIdentityConverter : IdentityConverter<StoryAudioId>
{
    public StoryAudioIdentityConverter() :
        base((x) => new StoryAudioId(x))
    {
    }
}
