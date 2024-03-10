using System.Windows.Input;

namespace Graphify.Client.Model.Shortcuts;

public record struct KeyCombination(Key Key, ModifierKeys Modifiers);
