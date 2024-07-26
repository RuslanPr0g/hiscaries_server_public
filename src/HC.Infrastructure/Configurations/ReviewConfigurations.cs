using HC.Domain.Users;
using HC.Infrastructure.Configurations.Converters;
using HC.Infrastructure.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HC.Infrastructure.Configurations;

internal class ReviewConfigurations : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ConfigureEntity<Review, ReviewId>();
        builder.Property(c => c.PublisherId).HasConversion(new IdentityConverter());
        builder.Property(c => c.ReviewerId).HasConversion(new IdentityConverter());

        builder
            .HasOne(r => r.Reviewer)
            .WithMany()
            .HasForeignKey(r => r.ReviewerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
