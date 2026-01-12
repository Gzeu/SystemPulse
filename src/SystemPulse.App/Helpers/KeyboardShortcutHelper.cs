using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Windows.System;

namespace SystemPulse.App.Helpers;

public class KeyboardShortcutHelper
{
    private readonly Window _window;
    private readonly Dictionary<string, Action> _shortcuts;

    public KeyboardShortcutHelper(Window window)
    {
        _window = window ?? throw new ArgumentNullException(nameof(window));
        _shortcuts = new Dictionary<string, Action>();
    }

    public void Initialize()
    {
        _window.Content.KeyDown += OnKeyDown;
    }

    public void RegisterShortcut(VirtualKey key, VirtualKeyModifiers modifiers, Action action)
    {
        var shortcutKey = GetShortcutKey(key, modifiers);
        _shortcuts[shortcutKey] = action;
    }

    public void RegisterShortcut(VirtualKey key, Action action)
    {
        RegisterShortcut(key, VirtualKeyModifiers.None, action);
    }

    public void UnregisterShortcut(VirtualKey key, VirtualKeyModifiers modifiers)
    {
        var shortcutKey = GetShortcutKey(key, modifiers);
        _shortcuts.Remove(shortcutKey);
    }

    public void ClearAllShortcuts()
    {
        _shortcuts.Clear();
    }

    private void OnKeyDown(object sender, KeyRoutedEventArgs e)
    {
        var modifiers = GetCurrentModifiers();
        var shortcutKey = GetShortcutKey(e.Key, modifiers);

        if (_shortcuts.TryGetValue(shortcutKey, out var action))
        {
            e.Handled = true;
            action?.Invoke();
        }
    }

    private VirtualKeyModifiers GetCurrentModifiers()
    {
        var modifiers = VirtualKeyModifiers.None;
        var coreWindow = Microsoft.UI.Xaml.Window.Current?.CoreWindow;

        if (coreWindow != null)
        {
            var ctrlState = coreWindow.GetKeyState(VirtualKey.Control);
            var shiftState = coreWindow.GetKeyState(VirtualKey.Shift);
            var altState = coreWindow.GetKeyState(VirtualKey.Menu);
            var winState = coreWindow.GetKeyState(VirtualKey.LeftWindows);

            if (ctrlState.HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down))
                modifiers |= VirtualKeyModifiers.Control;
            if (shiftState.HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down))
                modifiers |= VirtualKeyModifiers.Shift;
            if (altState.HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down))
                modifiers |= VirtualKeyModifiers.Menu;
            if (winState.HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down))
                modifiers |= VirtualKeyModifiers.Windows;
        }

        return modifiers;
    }

    private string GetShortcutKey(VirtualKey key, VirtualKeyModifiers modifiers)
    {
        return $"{modifiers}+{key}";
    }

    public string GetShortcutDisplayText(VirtualKey key, VirtualKeyModifiers modifiers)
    {
        var parts = new List<string>();

        if (modifiers.HasFlag(VirtualKeyModifiers.Control))
            parts.Add("Ctrl");
        if (modifiers.HasFlag(VirtualKeyModifiers.Shift))
            parts.Add("Shift");
        if (modifiers.HasFlag(VirtualKeyModifiers.Menu))
            parts.Add("Alt");
        if (modifiers.HasFlag(VirtualKeyModifiers.Windows))
            parts.Add("Win");

        parts.Add(key.ToString());

        return string.Join("+", parts);
    }
}
