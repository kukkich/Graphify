using Graphify.Client.Model.Commands;
using Graphify.Client.Model.Draw;
using Graphify.Client.Model.Tools;
using Graphify.Core.Model.IO.Export;
using Graphify.Core.Model.IO.Import;
using Microsoft.Extensions.DependencyInjection;

namespace Graphify.Client.Model;

public class Application
{
    public ApplicationContext Context { get; }
    public Exporter Exporter { get; }
    public Importer Importer { get; }
    public ToolsController ToolsController { get; }
    public CommandsBuffer CommandsBuffer { get; }

    private readonly DrawLoop _drawLoop;

    private readonly Clipboard _clipboard;

    public Application(IServiceProvider serviceProvider)
    {
        Context = serviceProvider.GetRequiredService<ApplicationContext>();
        Exporter = serviceProvider.GetRequiredService<Exporter>();
        Importer = serviceProvider.GetRequiredService<Importer>();

        ToolsController = serviceProvider.GetRequiredService<ToolsController>();
        CommandsBuffer = serviceProvider.GetRequiredService<CommandsBuffer>();

        _drawLoop = serviceProvider.GetRequiredService<DrawLoop>();
        _drawLoop.Initialize(160);
        _drawLoop.Start();

        _clipboard = serviceProvider.GetRequiredService<Clipboard>();
    }

    public void Copy()
    {
        _clipboard.CopyObjects(Context.SelectedObjects);
        CommandsBuffer.AddCommand(new CopyCommand(_clipboard, Context.SelectedObjects.ToHashSet()));
    }

    public void Cut()
    {
        _clipboard.CopyObjects(Context.SelectedObjects);

        foreach (var geometricObject in Context.SelectedObjects)
        {
            Context.Surface.TryRemove(geometricObject);
        }

        CommandsBuffer.AddCommand(new CutCommand(Context, _clipboard, Context.SelectedObjects.ToHashSet()));
    }

    public void Paste()
    {
        var pastedObjects = _clipboard.PasteObjects();

        foreach (var geometricObject in pastedObjects)
        {
            Context.Surface.AddObject(geometricObject);
        }

        CommandsBuffer.AddCommand(new PasteCommand(Context, _clipboard));
    }

    public void Delete()
    {
        foreach (var geometricObject in Context.SelectedObjects)
        {
            Context.Surface.TryRemove(geometricObject);
        }

        CommandsBuffer.AddCommand(new DeleteCommand(Context, Context.SelectedObjects.ToHashSet()));
    }

    public void Clear()
    {
        Context.Surface.Clear();
        Context.ClearSelected();
    }
}
