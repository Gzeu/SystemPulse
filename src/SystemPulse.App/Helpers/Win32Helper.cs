using System.Runtime.InteropServices;
using Microsoft.UI.Xaml;
using WinRT.Interop;

namespace SystemPulse.App.Helpers;

public static class Win32Helper
{
    #region Constants
    private const int GWL_EXSTYLE = -20;
    private const int WS_EX_LAYERED = 0x80000;
    private const int LWA_ALPHA = 0x2;
    private const uint SWP_NOMOVE = 0x0002;
    private const uint SWP_NOSIZE = 0x0001;
    #endregion

    #region P/Invoke Declarations

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    #endregion

    #region Window Handle Helper

    /// <summary>
    /// Gets the HWND for a WinUI 3 Window
    /// </summary>
    public static IntPtr GetWindowHandle(Window window)
    {
        if (window == null)
            throw new ArgumentNullException(nameof(window));

        return WindowNative.GetWindowHandle(window);
    }

    #endregion

    #region Window Opacity

    /// <summary>
    /// Sets the opacity of a window
    /// </summary>
    /// <param name="window">The window to modify</param>
    /// <param name="opacity">Opacity value from 0.0 (transparent) to 1.0 (opaque)</param>
    /// <returns>True if successful, false otherwise</returns>
    public static bool SetWindowOpacity(Window window, double opacity)
    {
        try
        {
            if (opacity < 0.0 || opacity > 1.0)
                throw new ArgumentOutOfRangeException(nameof(opacity), "Opacity must be between 0.0 and 1.0");

            var hwnd = GetWindowHandle(window);

            // Get current extended style
            int exStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            if (exStyle == 0)
            {
                var error = Marshal.GetLastWin32Error();
                System.Diagnostics.Debug.WriteLine($"GetWindowLong failed with error: {error}");
                return false;
            }

            // Add layered window style if not present
            if ((exStyle & WS_EX_LAYERED) == 0)
            {
                int newStyle = exStyle | WS_EX_LAYERED;
                if (SetWindowLong(hwnd, GWL_EXSTYLE, newStyle) == 0)
                {
                    var error = Marshal.GetLastWin32Error();
                    System.Diagnostics.Debug.WriteLine($"SetWindowLong failed with error: {error}");
                    return false;
                }
            }

            // Set the opacity
            byte alpha = (byte)(opacity * 255);
            bool result = SetLayeredWindowAttributes(hwnd, 0, alpha, LWA_ALPHA);

            if (!result)
            {
                var error = Marshal.GetLastWin32Error();
                System.Diagnostics.Debug.WriteLine($"SetLayeredWindowAttributes failed with error: {error}");
            }

            return result;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"SetWindowOpacity exception: {ex.Message}");
            return false;
        }
    }

    #endregion

    #region Always On Top

    private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
    private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);

    /// <summary>
    /// Sets whether a window should always be on top
    /// </summary>
    /// <param name="window">The window to modify</param>
    /// <param name="alwaysOnTop">True to keep window on top, false otherwise</param>
    /// <returns>True if successful, false otherwise</returns>
    public static bool SetAlwaysOnTop(Window window, bool alwaysOnTop)
    {
        try
        {
            var hwnd = GetWindowHandle(window);

            // Set window position flags
            bool result = SetWindowPos(
                hwnd,
                alwaysOnTop ? HWND_TOPMOST : HWND_NOTOPMOST,
                0, 0, 0, 0,
                SWP_NOMOVE | SWP_NOSIZE
            );

            if (!result)
            {
                var error = Marshal.GetLastWin32Error();
                System.Diagnostics.Debug.WriteLine($"SetWindowPos failed with error: {error}");
            }

            return result;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"SetAlwaysOnTop exception: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Checks if a window is currently set to always be on top
    /// </summary>
    public static bool IsAlwaysOnTop(Window window)
    {
        try
        {
            var hwnd = GetWindowHandle(window);
            int exStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            const int WS_EX_TOPMOST = 0x00000008;
            return (exStyle & WS_EX_TOPMOST) != 0;
        }
        catch
        {
            return false;
        }
    }

    #endregion

    #region Window Activation

    /// <summary>
    /// Brings a window to the foreground
    /// </summary>
    public static bool BringToForeground(Window window)
    {
        try
        {
            var hwnd = GetWindowHandle(window);
            return SetForegroundWindow(hwnd);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"BringToForeground exception: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Checks if a window is currently in the foreground
    /// </summary>
    public static bool IsForegroundWindow(Window window)
    {
        try
        {
            var hwnd = GetWindowHandle(window);
            return GetForegroundWindow() == hwnd;
        }
        catch
        {
            return false;
        }
    }

    #endregion

    #region Utility Methods

    /// <summary>
    /// Tests if Win32 API calls are working
    /// </summary>
    public static bool TestWin32Support(Window window)
    {
        try
        {
            var hwnd = GetWindowHandle(window);
            return hwnd != IntPtr.Zero;
        }
        catch
        {
            return false;
        }
    }

    #endregion
}
