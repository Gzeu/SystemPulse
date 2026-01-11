using Microsoft.UI.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SystemPulse.App.Services;
using SystemPulse.App.ViewModels;
using SystemPulse.App.Helpers;

namespace SystemPulse.App;

public sealed partial class App : Application
{
    private IServiceProvider _serviceProvider;
    private MainWindow m_window;

    public App()
    {
        InitializeComponent();
        SetupLogging();
        SetupDependencyInjection();
    }

    private void SetupLogging()
    {
        var appDataDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "SystemPulse", "logs");

        Directory.CreateDirectory(appDataDir);

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Debug()
            .WriteTo.File(
                Path.Combine(appDataDir, "app.log"),
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 30)
            .CreateLogger();

        Log.Information("SystemPulse Application starting... Version: 0.2.0 (Phase 3 - ProcessesPage)");
    }

    private void SetupDependencyInjection()
    {
        var services = new ServiceCollection();

        // Register Core Services
        services.AddSingleton<ILoggingService, LoggingService>();
        services.AddSingleton<ISettingsService, SettingsService>();
        services.AddSingleton<ISystemMonitorService, SystemMonitorService>();
        services.AddSingleton<IProcessService, ProcessService>();
        services.AddSingleton<IWMIService, WMIService>();

        // Register ViewModels
        services.AddSingleton<ShellViewModel>();
        services.AddSingleton<OverviewViewModel>();
        services.AddSingleton<ProcessesViewModel>();
        services.AddSingleton<PerformanceViewModel>();
        services.AddSingleton<SettingsViewModel>();
        services.AddSingleton<ServiceManagementViewModel>();
        services.AddSingleton<StartupAppsViewModel>();
        services.AddSingleton<UsersViewModel>();

        // Register Helpers
        services.AddSingleton<ThemeHelper>();
        services.AddSingleton<ChartDataHelper>();
        services.AddSingleton<DialogHelper>();
        services.AddSingleton<ExportHelper>();

        _serviceProvider = services.BuildServiceProvider();

        Log.Information("Dependency injection container configured successfully");
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        m_window = new MainWindow();
        m_window.Activate();
        
        // Set theme helper reference
        var themeHelper = _serviceProvider.GetService<ThemeHelper>();
        themeHelper?.SetWindowReference(m_window);
        
        Log.Information("Application window activated");
    }

    public IServiceProvider Services => _serviceProvider;
}
