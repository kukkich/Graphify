using System.IO;
using Graphify.Client.Model;
using Graphify.Client.Model.Commands;
using Graphify.Client.Model.Draw;
using Graphify.Client.Model.Geometry;
using Graphify.Client.Model.Interfaces;
using Graphify.Client.Model.Tools;
using Graphify.Client.View.Drawing;
using Graphify.Client.ViewModel;
using Graphify.Core.Model.IO.Export;
using Graphify.Core.Model.IO.Import;
using Graphify.Geometry.Drawing;
using Graphify.Geometry.GeometricObjects;
using Graphify.Geometry.GeometricObjects.Interfaces;
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
        services.AddScoped<OpenGLDrawer>();

        ConfigureExportImport(services);
        ConfigureApplication(services);
        ConfigureTools(services);
    }

    private static void ConfigureExportImport(IServiceCollection services)
    {
        services.AddSingleton<Exporter>();
        services.AddSingleton<Importer>();

        services.AddSingleton<IImporterFactory, ImporterFactory>();
        services.AddSingleton<IExporterFactory, ExporterFactory>();
    }

    private static void ConfigureApplication(IServiceCollection services)
    {
        services.AddSingleton<Application>();
        services.AddSingleton<ApplicationContext>();
        services.AddScoped<DrawLoop>();
        services.AddScoped<Surface>();
        services.AddScoped<IGeometryFactory, GeometryFactory>();
        services.AddScoped<IDrawer, OpenGLDrawer>();

        services.AddScoped<CommandsBuffer>();
    }
    
    private static void ConfigureTools(IServiceCollection services)
    {
        services.AddScoped<ToolsController>();
        services.AddScoped<IToolsFactory, ToolsFactory>();
    }
}
