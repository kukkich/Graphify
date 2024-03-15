namespace Graphify.Client.Model.Commands;

public class CommandsBuffer
{
    private const int StackSize = 10;
    private readonly LinkedList<ICommand> _commands;
    private readonly LinkedList<ICommand> _cancelledCommands;

    public CommandsBuffer()
    {
        _commands = new LinkedList<ICommand>();
        _cancelledCommands = new LinkedList<ICommand>();
    }

    public void AddCommand(ICommand command)
    {
        if (_commands.Count >= StackSize)
        {
            _commands.RemoveLast();
            
        }

        _commands.AddFirst(command);
        _cancelledCommands.Clear();
    }

    public void Undo()
    {
        if (_commands.Count <= 0) return;

        var command = _commands.First.Value;
        _commands.RemoveFirst();
        command.Undo();

        if (_cancelledCommands.Count < StackSize)
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
