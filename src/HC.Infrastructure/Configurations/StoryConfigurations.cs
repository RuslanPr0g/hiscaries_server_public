using HC.Domain.Stories;
using HC.Infrastructure.Configurations.Converters;
using HC.Infrastructure.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HC.Infrastructure.Configurations;

internal class StoryConfigurations : IEntityTypeConfiguration<Story>
{
    public void Configure(EntityTypeBuilder<Story> builder)
    {
        builder.ConfigureEntity<Story, StoryId, StoryIdentityConverter>();
        builder.Property(c => c.PublisherId).HasConversion(new UserIdentityConverter());

        builder
            .HasOne(s => s.Publisher)
            .WithMany()
            .HasForeignKey(s => s.PublisherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(s => s.Genres)
            .WithMany()
            .UsingEntity(j => j.ToTable("StoryGenres"));

        builder
            .HasMany(s => s.Contents)
            .WithOne()
            .HasForeignKey(sp => sp.StoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(s => s.Comments)
            .WithOne(c => c.Story)
            .HasForeignKey(c => c.StoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(s => s.Audios)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(s => s.Ratings)
            .WithOne()
            .HasForeignKey(sr => sr.StoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
