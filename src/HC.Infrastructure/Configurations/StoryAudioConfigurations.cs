using HC.Domain.Stories;
using HC.Infrastructure.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HC.Infrastructure.Configurations;

internal class StoryAudioConfigurations : IEntityTypeConfiguration<StoryAudio>
{
    public void Configure(EntityTypeBuilder<StoryAudio> builder)
    {
        builder.ConfigureEntity<StoryAudio, StoryAudioId>();
    }
}
