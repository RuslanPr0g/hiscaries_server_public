using HC.Application.Interface;
using HC.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HC.Application.Common.Extentions;

public static class ServicesConfiguration
{
    public static IServiceCollection AddServicesServices(this IServiceCollection services)
    {
        services.AddScoped<IUserWriteService, UserWriteService>();
        services.AddScoped<IStoryWriteService, StoryWriteService>();
        services.AddScoped<IUserReadService, UserReadService>();
        services.AddScoped<IStoryReadService, StoryReadService>();
        return services;
    }
}