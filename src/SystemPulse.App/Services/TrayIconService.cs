using System.Drawing;
using System.Windows.Forms;
using Microsoft.UI.Xaml;
using SystemPulse.App.Helpers;

namespace SystemPulse.App.Services;

public interface ITrayIconService
{
    void Initialize(Window mainWindow);
    void ShowNotification(string title, string message, ToolTipIcon icon = ToolTipIcon.Info);
    void Dispose();
}

public class TrayIconService : ITrayIconService
{
    private NotifyIcon? _notifyIcon;
    private Window? _mainWindow;
    private bool _isDisposed;

    public void Initialize(Window mainWindow)
    {
        if (_notifyIcon != null)
            return;

        _mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));

        // Create the NotifyIcon
        _notifyIcon = new NotifyIcon
        {
            Text = "SystemPulse - System Monitor",
            Visible = true
        };

        // Set icon (using system icon as placeholder)
        _notifyIcon.Icon = SystemIcons.Application;

        // Double-click to show/hide
        _notifyIcon.DoubleClick += (s, e) => ToggleWindowVisibility();

        // Create context menu
        var contextMenu = new ContextMenuStrip();
        
        contextMenu.Items.Add("Show", null, (s, e) => ShowWindow());
        contextMenu.Items.Add("Hide", null, (s, e) => HideWindow());
        contextMenu.Items.Add(new ToolStripSeparator());
        contextMenu.Items.Add("Exit", null, (s, e) => ExitApplication());

        _notifyIcon.ContextMenuStrip = contextMenu;

        System.Diagnostics.Debug.WriteLine("Tray icon initialized");
    }

    public void ShowNotification(string title, string message, ToolTipIcon icon = ToolTipIcon.Info)
    {
        if (_notifyIcon == null || _isDisposed)
            return;

        _notifyIcon.ShowBalloonTip(3000, title, message, icon);
    }

    private void ToggleWindowVisibility()
    {
        if (_mainWindow == null)
            return;

        // Note: WinUI 3 visibility toggle requires different approach
        // This is a simplified version
        ShowWindow();
    }

    private void ShowWindow()
    {
        if (_mainWindow == null)
            return;

        _mainWindow.DispatcherQueue.TryEnqueue(() =>
        {
            _mainWindow.Activate();
            Win32Helper.BringToForeground(_mainWindow);
        });
    }

    private void HideWindow()
    {
        if (_mainWindow == null)
            return;

        // WinUI 3 doesn't support hiding windows easily
        // This would need additional Win32 API calls
        System.Diagnostics.Debug.WriteLine("Hide window requested (not fully implemented)");
    }

    private void ExitApplication()
    {
        _mainWindow?.DispatcherQueue.TryEnqueue(() =>
        {
            System.Windows.Forms.Application.Exit();
        });
    }

    public void Dispose()
    {
        if (_isDisposed)
            return;

        _notifyIcon?.Dispose();
        _notifyIcon = null;
        _isDisposed = true;

        System.Diagnostics.Debug.WriteLine("Tray icon disposed");
    }
}
