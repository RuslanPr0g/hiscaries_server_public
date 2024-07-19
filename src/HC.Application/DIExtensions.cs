using HC.Application.Generators;
using HC.Application.Interface;
using HC.Application.Interface.Generators;
using HC.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HC.Application;

public static class DIExtensions
{
    public static IServiceCollection AddServicesServices(this IServiceCollection services)
    {
        services.AddScoped<IUserWriteService, UserWriteService>();
        services.AddScoped<IStoryWriteService, StoryWriteService>();
        services.AddScoped<IUserReadService, UserReadService>();
        services.AddScoped<IStoryReadService, StoryReadService>();
        services.AddScoped<IIdGenerator, IdGenerator>();
        return services;
    }
}