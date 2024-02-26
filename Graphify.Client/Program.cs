using System.IO;
using Graphify.Client.ViewModel;
using Graphify.Core;
using Graphify.Core.Geometry;
using Graphify.Core.IO;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Graphify.Client;

public class Program
{
    [STAThread]
    public static void Main()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        var provider = services.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateOnBuild = true
        });

        var app = provider.GetRequiredService<App>();

        try
        {
            app?.Run();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        services.AddSingleton(configuration);

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        services.AddLogging(loggingBuilder =>
            loggingBuilder.AddSerilog(dispose: true));

        services.AddSingleton<App>();
        services.AddSingleton<MainWindow>();
        services.AddTransient<AppViewModel>();

        ConfigureApplication(services);
    }

    private static void ConfigureApplication(IServiceCollection services)
    {
        services.AddSingleton<Application>();
        services.AddSingleton<GeometryFactory>();
    }
}
