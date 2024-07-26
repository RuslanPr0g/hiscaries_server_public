using HC.Domain.Stories;
using HC.Infrastructure.Configurations.Converters;
using HC.Infrastructure.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HC.Infrastructure.Configurations;

internal class StoryRatingConfigurations : IEntityTypeConfiguration<StoryRating>
{
    public void Configure(EntityTypeBuilder<StoryRating> builder)
    {
        builder.ConfigureEntity<StoryRating, StoryRatingId>();
        builder.Property(c => c.UserId).HasConversion(new IdentityConverter());
        builder.Property(c => c.StoryId).HasConversion(new IdentityConverter());

        builder
            .HasOne(sr => sr.User)
            .WithMany()
            .HasForeignKey(sr => sr.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
