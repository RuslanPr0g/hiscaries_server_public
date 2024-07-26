using HC.Domain.Stories;
using HC.Infrastructure.Configurations.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HC.Infrastructure.Configurations;

internal class StoryPageConfigurations : IEntityTypeConfiguration<StoryPage>
{
    public void Configure(EntityTypeBuilder<StoryPage> builder)
    {
        builder.HasKey(sp => new { sp.StoryId, sp.Page });
        builder.Property(c => c.StoryId).HasConversion(new IdentityConverter());
    }
}
