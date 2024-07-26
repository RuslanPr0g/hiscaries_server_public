using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace HC.API;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            using IHost host = CreateHostBuilder(args).Build();
            host.Run();
        }
        catch (Exception ex)
        {
            if (Log.Logger == null || Log.Logger.GetType().Name == "SilentLogger")
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.Console()
                    .CreateLogger();

            Log.Fatal(ex, "Host terminated unexpectedly");
            throw;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        IHostBuilder builder = Host.CreateDefaultBuilder(args);
        return builder.ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder
                .UseStartup<Startup>();
        });
    }
}