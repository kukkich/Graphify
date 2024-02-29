using System.IO;
using Graphify.Client.View.Drawing;
using Graphify.Client.Model;
using Graphify.Client.Model.Commands;
using Graphify.Client.Model.Geometry;
using Graphify.Client.ViewModel;
using Graphify.Geometry.GeometricObjects;
using Graphify.IO;
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
       
        services.AddIO();
        services.AddSingleton<OpenGLDrawer>();

        ConfigureApplication(services);
    }

    private static void ConfigureApplication(IServiceCollection services)
    {
        services.AddSingleton<ApplicationContext>();
        services.AddSingleton<Surface>();
        services.AddSingleton<IGeometryFactory, GeometryFactory>();
        services.AddScoped<CommandsBuffer>();
        services.AddSingleton<Application>();
    }
}
