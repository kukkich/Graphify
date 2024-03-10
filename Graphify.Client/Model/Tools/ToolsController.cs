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

    public void SetTool(EditMode editMode)
    {
        if (_currentTool is not null)
        {
            _currentTool.Reset();
        }

        _currentTool = GetTool(editMode);
    }

    private IApplicationTool GetTool(EditMode editMode)
    {
        return _tools[editMode];
    }

    public void MouseDown(Vector2 position)
    {
        if (_currentTool is not null)
        {
            _currentTool.MouseDown(position);
        }
    }

    public void MouseMove(Vector2 newPosition)
    {
        if (_currentTool is not null)
        {
            _currentTool.MouseMove(newPosition);
        }
    }
}
