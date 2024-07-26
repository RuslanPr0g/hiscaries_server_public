using HC.Domain.Users;
using HC.Infrastructure.Configurations.Converters;
using HC.Infrastructure.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HC.Infrastructure.Configurations;

public class UserReadHistoryConfigurations : IEntityTypeConfiguration<UserReadHistory>
{
    public void Configure(EntityTypeBuilder<UserReadHistory> builder)
    {
        builder.ConfigureEntity<UserReadHistory, UserReadHistoryId, UserReadHistoryIdentityConverter>();
        builder.Property(c => c.UserId).HasConversion(new UserIdentityConverter());
        builder.Property(c => c.StoryId).HasConversion(new StoryIdentityConverter());

        builder
            .HasOne(urh => urh.Story)
            .WithMany()
            .HasForeignKey(urh => urh.StoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
