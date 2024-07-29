using HC.Application.Interface;
using HC.Application.Interface.DataAccess;
using HC.Infrastructure.DataAccess;
using HC.Infrastructure.Repository;
using HC.Infrastructure.UnitOfWorks;
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

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        var mainConnectionString = connection.GetConnectionString("PostgresEF");
        services.AddDbContext<HiscaryContext>(options =>
        {
            options.UseNpgsql(mainConnectionString, b => { b.MigrationsAssembly("HC.Infrastructure"); });
        });

        using (var scope = services.BuildServiceProvider().CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<HiscaryContext>();
            db.Database.Migrate();
        }

        return services;
    }
}