using HC.Domain.Stories;
using HC.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace HC.Infrastructure.DataAccess;

public class HiscaryContext : DbContext
{
    public HiscaryContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Story> Stories { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}