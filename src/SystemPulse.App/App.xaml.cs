using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using SystemPulse.App.Helpers;
using SystemPulse.App.Services;
using SystemPulse.App.ViewModels;
using System;

namespace SystemPulse.App;

public partial class App : Application
{
    public IServiceProvider Services { get; private set; }
    private Window? _window;

    public App()
    {
        InitializeComponent();
        Services = ConfigureServices();
    }

    private IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        // Register Services
        services.AddSingleton<ILoggingService, LoggingService>();
        services.AddSingleton<ISystemMonitorService, SystemMonitorService>();
        services.AddSingleton<ITrayIconService, TrayIconService>();
        services.AddSingleton<IToastNotificationService, ToastNotificationService>();

        // Register ViewModels
        services.AddTransient<OverviewViewModel>();
        services.AddTransient<ProcessesViewModel>();
        services.AddTransient<PerformanceViewModel>();
        services.AddSingleton<SettingsViewModel>(); // Singleton to maintain state
        services.AddTransient<ServicesViewModel>();
        services.AddTransient<StartupViewModel>();
        services.AddTransient<UsersViewModel>();
        services.AddTransient<DetailsViewModel>();

        return services.BuildServiceProvider();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        _window = new MainWindow();
        _window.Activate();

        var logger = Services.GetRequiredService<ILoggingService>();
        logger.LogInfo("SystemPulse application started");
        logger.LogInfo($"Platform: {Environment.OSVersion}");
        logger.LogInfo($".NET Version: {Environment.Version}");
        logger.LogInfo("All services initialized successfully");
    }
}
