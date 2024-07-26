using HC.Domain.Stories;
using HC.Infrastructure.Configurations.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HC.Infrastructure.Configurations;

public class StoryPageConfigurations : IEntityTypeConfiguration<StoryPage>
{
    public void Configure(EntityTypeBuilder<StoryPage> builder)
    {
        builder.HasKey(sp => new { sp.StoryId, sp.Page });
        builder.Property(c => c.StoryId).HasConversion(new StoryIdentityConverter());
    }
}
