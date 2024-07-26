using HC.Domain;
using HC.Infrastructure.Configurations.Converters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HC.Infrastructure.Extentions;

internal static class ConfigurationExtensions
{
    internal static EntityTypeBuilder<TEntity> ConfigureEntity<TEntity, TIdentity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : Entity<TIdentity>
        where TIdentity : Identity
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasConversion(new IdentityConverter());
        return builder;
    }
}
