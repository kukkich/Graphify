using System.Windows.Input;

namespace Graphify.Client.Model.Shortcuts;

public class ShortcutController
{
    public bool ShortcutExecutionBlocked => _shortcutExecutionBlockers.Count > 0;

    private readonly HashSet<string> _shortcutExecutionBlockers = [];

    private readonly Dictionary<KeyCombination, Action> _commands = [];
    
    public void BlockShortcutExecution(string blocker)
    {
        _shortcutExecutionBlockers.Add(blocker);
    }

    public void UnblockShortcutExecution(string blocker)
    {
        _shortcutExecutionBlockers.Remove(blocker);
    }
    
    public void UnblockShortcutExecutionAll()
    {
        _shortcutExecutionBlockers.Clear();
    }

    public void AddCommand(KeyCombination keyCombination, Action action)
    {
        _commands.Add(keyCombination, action);
    }
    
    public void KeyPressed(Key key, ModifierKeys modifiers)
    {
        KeyCombination shortcut = new(key, modifiers);

        if (!ShortcutExecutionBlocked)
        {
            if (_commands.TryGetValue(shortcut, out Action action))
            {
                action();
            }
        }
    }
}
