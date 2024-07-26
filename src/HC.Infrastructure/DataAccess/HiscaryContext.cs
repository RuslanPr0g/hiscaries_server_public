using HC.Domain.Stories;
using HC.Domain.Users;
using HC.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace HC.Infrastructure.DataAccess;

public class DatabaseDesignTimeDbContextFactory
    : IDesignTimeDbContextFactory<HiscaryContext>
{
    public HiscaryContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

        var connectionString = configuration.GetConnectionString("PostgresEF");

        var builder = new DbContextOptionsBuilder<HiscaryContext>();
        builder.UseNpgsql(connectionString);
        return new HiscaryContext(builder.Options);
    }
}

public sealed class HiscaryContext(DbContextOptions<HiscaryContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserReadHistory> ReadHistory { get; set; }
    public DbSet<Story> Stories { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfigurations).Assembly);
    }
}