using Microsoft.UI.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SystemPulse.App.Services;
using SystemPulse.App.ViewModels;

namespace SystemPulse.App;

public sealed partial class App : Application
{
    private IServiceProvider _serviceProvider;

    public App()
    {
        InitializeComponent();
        SetupLogging();
        SetupDependencyInjection();
    }

    private void SetupLogging()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Debug()
            .WriteTo.File(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "SystemPulse", "logs", "app.log"),
                rollingInterval: RollingInterval.Day)
            .CreateLogger();

        Log.Information("Application starting...");
    }

    private void SetupDependencyInjection()
    {
        var services = new ServiceCollection();

        // Register services
        services.AddSingleton<ILoggingService, LoggingService>();
        services.AddSingleton<ISystemMonitorService, SystemMonitorService>();
        services.AddSingleton<IProcessService, ProcessService>();
        services.AddSingleton<IWMIService, WMIService>();
        services.AddSingleton<ISettingsService, SettingsService>();

        // Register ViewModels
        services.AddSingleton<ShellViewModel>();
        services.AddSingleton<OverviewViewModel>();
        services.AddSingleton<ProcessesViewModel>();
        services.AddSingleton<PerformanceViewModel>();
        services.AddSingleton<SettingsViewModel>();

        _serviceProvider = services.BuildServiceProvider();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        m_window = new MainWindow();
        m_window.Activate();
    }

    private Window m_window;

    public IServiceProvider Services => _serviceProvider;
}
