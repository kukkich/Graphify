namespace Graphify.Client.Model.Commands;

public interface ICommand
{
    public void Execute();
    public void Undo();
}
