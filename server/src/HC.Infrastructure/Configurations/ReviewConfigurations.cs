using HC.Domain.Users;
using HC.Infrastructure.Configurations.Converters;
using HC.Infrastructure.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HC.Infrastructure.Configurations;

public class ReviewConfigurations : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ConfigureEntity<Review, ReviewId, ReviewIdentityConverter>();
        builder.Property(c => c.PublisherId).HasConversion(new UserIdentityConverter());
        builder.Property(c => c.ReviewerId).HasConversion(new UserIdentityConverter());

        builder
            .HasOne(r => r.Reviewer)
            .WithMany()
            .HasForeignKey(r => r.ReviewerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
