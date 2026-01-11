using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SystemPulse.App.Models;
using SystemPulse.App.Services;
using Microsoft.UI.Xaml;

namespace SystemPulse.App.ViewModels;

public partial class ShellViewModel : ObservableObject
{
    private readonly ISystemMonitorService _monitorService;
    private readonly ISettingsService _settingsService;
    private readonly ILoggingService _logger;
    private DispatcherTimer _updateTimer;

    [ObservableProperty]
    private PerformanceMetrics systemMetrics;

    [ObservableProperty]
    private string appTitle = "SystemPulse";

    [ObservableProperty]
    private string statusText = "Ready";

    [ObservableProperty]
    private bool isMonitoring;

    public ShellViewModel(
        ISystemMonitorService monitorService,
        ISettingsService settingsService,
        ILoggingService logger)
    {
        _monitorService = monitorService ?? throw new ArgumentNullException(nameof(monitorService));
        _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        InitializeMonitoring();
    }

    private void InitializeMonitoring()
    {
        _updateTimer = new DispatcherTimer();
        _updateTimer.Interval = TimeSpan.FromSeconds(2);
        _updateTimer.Tick += (s, e) => UpdateMetrics();
        _updateTimer.Start();

        IsMonitoring = true;
        _logger.LogInfo("Shell ViewModel initialized and monitoring started");
    }

    private void UpdateMetrics()
    {
        try
        {
            SystemMetrics = _monitorService.GetMetrics();
            StatusText = $"Updated: {DateTime.Now:HH:mm:ss}";
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to update metrics: {ex.Message}", ex);
            StatusText = "Error updating metrics";
        }
    }

    [RelayCommand]
    private void Shutdown()
    {
        _updateTimer?.Stop();
        _logger.LogInfo("Application shutting down");
    }
}
