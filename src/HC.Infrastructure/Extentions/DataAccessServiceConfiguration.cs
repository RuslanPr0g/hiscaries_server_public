using HC.Application.Interface;
using HC.Infrastructure.DataAccess;
using HC.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HC.Infrastructure.Extentions;

public static class DataAccessServiceConfiguration
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration connection)
    {
        services.AddScoped<IUserReadRepository, EFUserReadRepository>();
        services.AddScoped<IStoryReadRepository, EFStoryReadRepository>();

        services.AddScoped<IUserWriteRepository, EFUserWriteRepository>();
        services.AddScoped<IStoryWriteRepository, EFStoryWriteRepository>();

        string mainConnectionString = connection.GetConnectionString("PostgresEF");
        services.AddDbContext<HiscaryContext>(options =>
        {
            options.UseNpgsql(mainConnectionString, b => { b.MigrationsAssembly("HC.Infrastructure"); });
        });

        // Migrate on startup
        //using (var scope = host.Services.CreateScope())
        //{
        //    var db = scope.ServiceProvider.GetRequiredService<HiscaryContext>();
        //    db.Database.Migrate();
        //}

        return services;
    }
}