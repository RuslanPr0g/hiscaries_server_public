using FluentValidation.AspNetCore;
using HC.Application.Common.Extentions;
using HC.Application.Filters;
using HC.Application.Options;
using HC.Application.Users.Command.CreateUser;
using HC.Infrastructure.Extentions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HC.API;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDataAccess(Configuration);

        services.AddServicesServices();

        services.AddMediatR((cfg) =>
        {
            // TODO: it takes multiple assemblies, maybe separate read and write projects into two?
            cfg.RegisterServicesFromAssemblies(typeof(RegisterUserCommandHandler).Assembly);
        });

        services.AddLogging();

        services
            .AddMvc(options => { options.Filters.Add<ValidationFilter>(); })
            .AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<Startup>());

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
            });
        });

        services.AddControllers();

        services.AddApiVersioning(options => { options.AssumeDefaultVersionWhenUnspecified = true; });

        services.AddOptions<DbConnectionStrings>()
            .Bind(Configuration.GetSection("ConnectionStrings"));

        JwtSettings jwtSettings = new();
        Configuration.Bind(nameof(jwtSettings), jwtSettings);

        services.AddJwtBearerBasedSwaggerSupport(jwtSettings);

        services.AddHttpContextAccessor();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HC.API v1"));
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseCors("AllowAll");

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}