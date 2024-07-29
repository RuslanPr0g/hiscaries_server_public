using HC.Domain.Stories;
using HC.Infrastructure.Configurations.Converters;
using HC.Infrastructure.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HC.Infrastructure.Configurations;

public class GenreConfigurations : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.ConfigureEntity<Genre, GenreId, GenreIdentityConverter>();
    }
}
