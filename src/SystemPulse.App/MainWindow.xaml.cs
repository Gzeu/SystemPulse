using Microsoft.UI.Xaml;
using SystemPulse.App.Services;
using SystemPulse.App.ViewModels;
using SystemPulse.App.Views;
using Microsoft.Extensions.DependencyInjection;
using SystemPulse.App.Helpers;

namespace SystemPulse.App;

public sealed partial class MainWindow : Window
{
    private readonly ILoggingService _logger;
    private readonly ITrayIconService _trayIconService;

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

        Title = "SystemPulse";
        
        // Initialize window
        InitializeWindow();

        // Set default page
        ContentFrame.Navigate(typeof(OverviewPage));
        NavigationView.SelectedItem = NavigationView.MenuItems[0];

        _logger.LogInfo("MainWindow initialized");
    }

    private void InitializeWindow()
    {
        // Initialize tray icon
        _trayIconService.Initialize(this);

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
