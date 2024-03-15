using Graphify.Client.Model.Enums;

namespace Graphify.Client.Model.Interfaces;

public interface IToolsFactory
{
    IApplicationTool CreateTool(EditMode editMode);
}
