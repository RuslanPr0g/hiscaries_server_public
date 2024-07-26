using HC.Domain.Stories;

namespace HC.Infrastructure.Configurations.Converters;

internal class StoryAudioIdentityConverter : IdentityConverter<StoryAudioId>
{
    internal StoryAudioIdentityConverter() :
        base((x) => new StoryAudioId(x))
    {
    }
}
