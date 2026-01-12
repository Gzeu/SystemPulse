using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using SystemPulse.App.Services;
using SystemPulse.App.ViewModels;
using SystemPulse.App.Views;
using SystemPulse.App.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Windows.System;

namespace SystemPulse.App;

public sealed partial class MainWindow : Window
{
    private readonly ILoggingService _logger;
    private readonly ITrayIconService _trayIconService;
    private readonly IToastNotificationService _toastService;
    private readonly KeyboardShortcutHelper _shortcutHelper;

    public MainWindow()
    {
        InitializeComponent();

        var app = Application.Current as App;
        if (app == null)
        {
            throw new InvalidOperationException("Application instance is null");
        }

        _logger = app.Services.GetRequiredService<ILoggingService>();
        _trayIconService = app.Services.GetRequiredService<ITrayIconService>();
        _toastService = app.Services.GetRequiredService<IToastNotificationService>();
        _shortcutHelper = new KeyboardShortcutHelper(this);

        Title = "SystemPulse";
        
        // Initialize window
        InitializeWindow();

        // Initialize keyboard shortcuts
        InitializeKeyboardShortcuts();

        // Set default page
        ContentFrame.Navigate(typeof(OverviewPage));
        NavigationView.SelectedItem = NavigationView.MenuItems[0];

        _logger.LogInfo("MainWindow initialized");
    }

    private void InitializeWindow()
    {
        // Initialize tray icon
        _trayIconService.Initialize(this);

        // Initialize toast notifications
        _toastService.Initialize();

        // Apply saved settings
        var app = Application.Current as App;
        var settingsViewModel = app?.Services.GetService<SettingsViewModel>();
        if (settingsViewModel != null)
        {
            settingsViewModel.SetMainWindow(this);
            _logger.LogInfo("Window settings applied from SettingsViewModel");
        }

        // Test Win32 support
        bool win32Supported = Win32Helper.TestWin32Support(this);
        _logger.LogInfo($"Win32 API support: {win32Supported}");

        // Show welcome notification
        _toastService.ShowInfo("SystemPulse", "System monitoring started");
    }

    private void InitializeKeyboardShortcuts()
    {
        _shortcutHelper.Initialize();

        // Ctrl+R or F5: Refresh current page
        _shortcutHelper.RegisterShortcut(VirtualKey.R, VirtualKeyModifiers.Control, RefreshCurrentPage);
        _shortcutHelper.RegisterShortcut(VirtualKey.F5, RefreshCurrentPage);

        // Ctrl+F: Focus search (if available on current page)
        _shortcutHelper.RegisterShortcut(VirtualKey.F, VirtualKeyModifiers.Control, FocusSearch);

        // Ctrl+,: Open Settings
        _shortcutHelper.RegisterShortcut(VirtualKey.Number188, VirtualKeyModifiers.Control, OpenSettings); // , key

        // Ctrl+1 through Ctrl+9: Navigate to pages
        _shortcutHelper.RegisterShortcut(VirtualKey.Number1, VirtualKeyModifiers.Control, () => NavigateToPage(0));
        _shortcutHelper.RegisterShortcut(VirtualKey.Number2, VirtualKeyModifiers.Control, () => NavigateToPage(1));
        _shortcutHelper.RegisterShortcut(VirtualKey.Number3, VirtualKeyModifiers.Control, () => NavigateToPage(2));
        _shortcutHelper.RegisterShortcut(VirtualKey.Number4, VirtualKeyModifiers.Control, () => NavigateToPage(3));
        _shortcutHelper.RegisterShortcut(VirtualKey.Number5, VirtualKeyModifiers.Control, () => NavigateToPage(4));
        _shortcutHelper.RegisterShortcut(VirtualKey.Number6, VirtualKeyModifiers.Control, () => NavigateToPage(5));
        _shortcutHelper.RegisterShortcut(VirtualKey.Number7, VirtualKeyModifiers.Control, () => NavigateToPage(6));
        _shortcutHelper.RegisterShortcut(VirtualKey.Number8, VirtualKeyModifiers.Control, () => NavigateToPage(7));
        _shortcutHelper.RegisterShortcut(VirtualKey.Number9, VirtualKeyModifiers.Control, () => NavigateToPage(8));

        // Escape: Clear selection/search
        _shortcutHelper.RegisterShortcut(VirtualKey.Escape, ClearSelection);

        _logger.LogInfo("Keyboard shortcuts initialized");
    }

    private void RefreshCurrentPage()
    {
        _logger.LogInfo("Refresh shortcut triggered");
        // Trigger refresh on current page if it supports it
        _toastService.ShowInfo("Refresh", "Page refreshed");
    }

    private void FocusSearch()
    {
        _logger.LogInfo("Focus search shortcut triggered");
        // Focus search box if available on current page
    }

    private void OpenSettings()
    {
        _logger.LogInfo("Open settings shortcut triggered");
        NavigateToPageType(typeof(SettingsPage));
    }

    private void NavigateToPage(int index)
    {
        if (index >= 0 && index < NavigationView.MenuItems.Count)
        {
            NavigationView.SelectedItem = NavigationView.MenuItems[index];
            _logger.LogInfo($"Navigate to page {index} via shortcut");
        }
    }

    private void NavigateToPageType(Type pageType)
    {
        if (ContentFrame.CurrentSourcePageType != pageType)
        {
            ContentFrame.Navigate(pageType);
        }
    }

    private void ClearSelection()
    {
        _logger.LogInfo("Clear selection shortcut triggered");
        // Clear current selection/search
    }

    private void NavigationView_SelectionChanged(
        Microsoft.UI.Xaml.Controls.NavigationView sender,
        Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItem is Microsoft.UI.Xaml.Controls.NavigationViewItem item)
        {
            var tag = item.Tag?.ToString();
            Type? pageType = tag switch
            {
                "overview" => typeof(OverviewPage),
                "processes" => typeof(ProcessesPage),
                "performance" => typeof(PerformancePage),
                "services" => typeof(ServicesPage),
                "startup" => typeof(StartupPage),
                "users" => typeof(UsersPage),
                "details" => typeof(DetailsPage),
                "settings" => typeof(SettingsPage),
                "about" => typeof(AboutPage),
                _ => null
            };

            if (pageType != null && ContentFrame.CurrentSourcePageType != pageType)
            {
                ContentFrame.Navigate(pageType);
                _logger.LogInfo($"Navigated to {pageType.Name}");
            }
        }
    }
}
