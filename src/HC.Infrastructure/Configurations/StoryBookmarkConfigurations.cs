using HC.Domain.Users;
using HC.Infrastructure.Configurations.Converters;
using HC.Infrastructure.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HC.Infrastructure.Configurations;

internal class StoryBookmarkConfigurations : IEntityTypeConfiguration<UserStoryBookMark>
{
    public void Configure(EntityTypeBuilder<UserStoryBookMark> builder)
    {
        builder.ConfigureEntity<UserStoryBookMark, UserStoryBookMarkId>();
        builder.Property(c => c.StoryId).HasConversion(new IdentityConverter());
        builder.Property(c => c.UserId).HasConversion(new IdentityConverter());
    }
}
