using System.Numerics;
using Graphify.Client.Model.Enums;
using Graphify.Client.Model.Interfaces;

namespace Graphify.Client.Model.Tools;

public class ToolsController
{
    private readonly Dictionary<EditMode, IApplicationTool> _tools = [];
    private IApplicationTool? _currentTool;

    public ToolsController(IToolsFactory toolsFactory)
    {
        CreateTools(toolsFactory);
    }

    private void CreateTools(IToolsFactory toolsFactory)
    {
        foreach (EditMode type in Enum.GetValues(typeof(EditMode)))
        {
            _tools.Add(type, toolsFactory.CreateTool(type));
        }
    }

    public IApplicationTool ChangeTool(EditMode editMode)
    {
        if (_currentTool is not null)
        {
            if (_currentTool.InProgress())
            {
                _currentTool.Cancel();
            }
            
            _currentTool.Reset();
        }

        _currentTool = GetTool(editMode);
        return _currentTool;
    }

    private IApplicationTool GetTool(EditMode editMode)
    {
        if (_tools.TryGetValue(editMode, out IApplicationTool tool))
        {
            return tool;
        }

        throw new ArgumentException($"There is no such tool {editMode}");
    }
}
