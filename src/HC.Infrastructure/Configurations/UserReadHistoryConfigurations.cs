using HC.Domain.Users;
using HC.Infrastructure.Configurations.Converters;
using HC.Infrastructure.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HC.Infrastructure.Configurations;

internal class UserReadHistoryConfigurations : IEntityTypeConfiguration<UserReadHistory>
{
    public void Configure(EntityTypeBuilder<UserReadHistory> builder)
    {
        builder.ConfigureEntity<UserReadHistory, UserReadHistoryId>();
        builder.Property(c => c.UserId).HasConversion(new IdentityConverter());
        builder.Property(c => c.StoryId).HasConversion(new IdentityConverter());

        builder
            .HasOne(urh => urh.Story)
            .WithMany()
            .HasForeignKey(urh => urh.StoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
