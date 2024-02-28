using Graphify.Client.Model.Geometry;

namespace Graphify.Client.Model.Commands;

public class CommandsBuffer
{
    private readonly LinkedList<ICommand> _commands;
    private readonly LinkedList<ICommand> _cancelledCommands;

    public CommandsBuffer()
    {
        _commands = new LinkedList<ICommand>();
        _cancelledCommands = new LinkedList<ICommand>();
    }

    public void AddCommand(ICommand command)
    {
        if (_commands.Count >= 10)
        {
            _commands.RemoveLast();
        }

        _commands.AddFirst(command);
    }

    public void Undo()
    {
        if (_commands.Count <= 0) return;

        var command = _commands.First.Value;
        _commands.RemoveFirst();
        command.Undo();

        if (_cancelledCommands.Count < 10)
        {
            _cancelledCommands.AddFirst(command);
        }
    }

    public void Redo()
    {
        if (_cancelledCommands.Count <= 0) return;

        var command = _cancelledCommands.First.Value;
        _cancelledCommands.RemoveFirst();
        command.Execute();
        _commands.AddFirst(command);
    }
}
