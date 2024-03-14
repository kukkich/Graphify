using Graphify.Client.Model.Commands;
using Graphify.Client.Model.Enums;
using Graphify.Client.Model.Interfaces;
using Graphify.Client.Model.Tools.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Graphify.Client.Model.Tools;

public class ToolsFactory : IToolsFactory
{
    private readonly Dictionary<EditMode, Func<IApplicationTool>> _factoryMethods = [];

    public ToolsFactory(IServiceProvider serviceProvider)
    {
        InitializeFactoryMethods(serviceProvider);
    }

    private void InitializeFactoryMethods(IServiceProvider serviceProvider)
    {
        _factoryMethods.Add(EditMode.Move, () => new MoveTool(serviceProvider.GetRequiredService<ApplicationContext>(), serviceProvider.GetRequiredService<CommandsBuffer>()));
        _factoryMethods.Add(EditMode.Rotate, () => new RotateTool(serviceProvider.GetRequiredService<ApplicationContext>()));
        _factoryMethods.Add(EditMode.CreatePoint, () => new PointTool(serviceProvider.GetRequiredService<ApplicationContext>(), serviceProvider.GetRequiredService<CommandsBuffer>()));
        _factoryMethods.Add(EditMode.CreateLine, () => new LineTool(serviceProvider.GetRequiredService<ApplicationContext>(), serviceProvider.GetRequiredService<CommandsBuffer>()));
        _factoryMethods.Add(EditMode.CreateCircleTwoPoints, () => new CircleTwoPointsTool(serviceProvider.GetRequiredService<ApplicationContext>(), serviceProvider.GetRequiredService<CommandsBuffer>()));
    }

    public IApplicationTool CreateTool(EditMode editMode)
    {
        return _factoryMethods[editMode]();
    }
}
