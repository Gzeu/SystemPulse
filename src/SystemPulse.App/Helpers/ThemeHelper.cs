using Microsoft.UI.Xaml;
using SystemPulse.App.Services;

namespace SystemPulse.App.Helpers;

public static class ThemeHelper
{
    private static WeakReference<Window> _windowRef;

    public static void SetWindowReference(Window window)
    {
        _windowRef = new WeakReference<Window>(window);
    }

    public static void SetTheme(string theme)
    {
        if (!_windowRef.TryGetTarget(out var window))
            return;

        var elementTheme = theme switch
        {
            "Light" => ElementTheme.Light,
            "Dark" => ElementTheme.Dark,
            "System" => ElementTheme.Default,
            _ => ElementTheme.Default
        };

        if (window.Content is FrameworkElement root)
        {
            root.RequestedTheme = elementTheme;
        }
    }

    public static string GetCurrentTheme()
    {
        if (!_windowRef.TryGetTarget(out var window))
            return "System";

        if (window.Content is FrameworkElement root)
        {
            return root.RequestedTheme switch
            {
                ElementTheme.Light => "Light",
                ElementTheme.Dark => "Dark",
                _ => "System"
            };
        }

        return "System";
    }

    public static void ApplyThemeToApplication(string theme)
    {
        SetTheme(theme);
    }
}
