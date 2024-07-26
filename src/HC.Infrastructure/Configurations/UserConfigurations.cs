using HC.Domain.Users;
using HC.Infrastructure.Configurations.Converters;
using HC.Infrastructure.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HC.Infrastructure.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ConfigureEntity<User, UserId, UserIdentityConverter>();
        builder.Property(c => c.RefreshTokenId).HasConversion(new RefreshTokenIdentityConverter());

        builder.Property(x => x.Role).HasConversion(new UserRoleConverter());

        builder
            .HasMany(u => u.Reviews)
            .WithOne(r => r.Publisher)
            .HasForeignKey(r => r.PublisherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(u => u.BookMarks)
            .WithOne()
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(u => u.RefreshToken)
            .WithOne()
            .HasForeignKey<User>(u => u.RefreshTokenId);

        builder
            .HasMany(u => u.ReadHistory)
            .WithOne()
            .HasForeignKey(rh => rh.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
