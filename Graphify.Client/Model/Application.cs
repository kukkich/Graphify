using System.Numerics;
using System.Windows.Input;
using Graphify.Client.Model.Commands;
using Graphify.Client.Model.Draw;
using Graphify.Client.Model.Enums;
using Graphify.Client.Model.Shortcuts;
using Graphify.Client.Model.Tools;
using Graphify.Core.Model.IO.Export;
using Graphify.Core.Model.IO.Import;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;
using Microsoft.Extensions.DependencyInjection;

namespace Graphify.Client.Model;

public class Application
{
    public ApplicationContext Context { get; }
    public Exporter Exporter { get; }
    public Importer Importer { get; }
    public ToolsController ToolsController { get; }

    public CommandsBuffer CommandsBuffer { get; }
    public ShortcutController ShortcutController { get; }
    
    private readonly DrawLoop _drawLoop;

    public Application(IServiceProvider serviceProvider)
    {
        Context = serviceProvider.GetRequiredService<ApplicationContext>();
        Exporter = serviceProvider.GetRequiredService<Exporter>();
        Importer = serviceProvider.GetRequiredService<Importer>();
        
        ToolsController = serviceProvider.GetRequiredService<ToolsController>();
        CommandsBuffer = serviceProvider.GetRequiredService<CommandsBuffer>();
        ShortcutController = serviceProvider.GetRequiredService<ShortcutController>();
        
        _drawLoop = serviceProvider.GetRequiredService<DrawLoop>();
        _drawLoop.Initialize(160);
        _drawLoop.Start();

        SetupCommands();
    }

    private void SetupCommands()
    {
        ShortcutController.AddCommand(new KeyCombination(Key.Z, ModifierKeys.Control), () => {CommandsBuffer.Undo();});
        ShortcutController.AddCommand(new KeyCombination(Key.Y, ModifierKeys.Control), () => {CommandsBuffer.Redo();});
        
        ShortcutController.AddCommand(new KeyCombination(Key.L, ModifierKeys.None), () => {ToolsController.SetTool(EditMode.CreateLine);});
    }
}
