using HC.Domain;
using HC.Infrastructure.Configurations.Converters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HC.Infrastructure.Extentions;

public static class ConfigurationExtensions
{
    public static EntityTypeBuilder<TEntity> ConfigureEntity<TEntity, TIdentity, TIdentityConverter>(
        this EntityTypeBuilder<TEntity> builder, TIdentityConverter? converter = null)
        where TEntity : Entity<TIdentity>
        where TIdentity : Identity
        where TIdentityConverter : IdentityConverter<TIdentity>, new()
    {
        builder.HasKey(u => u.Id);
        if (converter is null)
        {
            builder.Property(u => u.Id).HasConversion(new TIdentityConverter()).ValueGeneratedOnAdd();
        }
        else
        {
            builder.Property(u => u.Id).HasConversion(converter).ValueGeneratedOnAdd();
        }
        return builder;
    }
}
