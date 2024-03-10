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
    }
}
