using System.Reflection;
using System.Windows;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using Splat;

namespace Graphify.Client;

public class App : Application
{
    private readonly MainWindow _mainWindow;
    private readonly ILogger<App> _logger;

    public App(MainWindow mainWindow, ILogger<App> logger)
    {
        _mainWindow = mainWindow;
        _logger = logger;
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        _logger.LogDebug("Application started");
        _mainWindow.Show();

        base.OnStartup(e);
    }
}
