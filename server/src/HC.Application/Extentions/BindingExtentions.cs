using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HC.Application.Extentions;

public static class BindingExtentions
{
    private static IServiceCollection BindConfigSection<T>(this IServiceCollection services,
        IConfiguration config, params string[] sectionNames) where T : class
    {
        _ = services.Configure<T>(options =>
        {
            IConfiguration currentConfig = config;
            foreach (string sectionName in sectionNames) currentConfig = currentConfig.GetSection(sectionName);
            currentConfig.Bind(options);
        });
        return services;
    }
}