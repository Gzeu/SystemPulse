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

    [ObservableProperty]
    private int refreshInterval = 2;

    public ShellViewModel(
        ISystemMonitorService monitorService,
        ISettingsService settingsService,
        ILoggingService logger)
    {
        _monitorService = monitorService ?? throw new ArgumentNullException(nameof(monitorService));
        _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        // Initialize with empty metrics
        SystemMetrics = new PerformanceMetrics
        {
            Timestamp = DateTime.Now
        };

        InitializeMonitoring();
    }

    private void InitializeMonitoring()
    {
        try
        {
            // Get refresh interval from settings
            RefreshInterval = _settingsService.GetRefreshInterval();

            _updateTimer = new DispatcherTimer();
            _updateTimer.Interval = TimeSpan.FromSeconds(RefreshInterval);
            _updateTimer.Tick += (s, e) => UpdateMetrics();
            _updateTimer.Start();

            IsMonitoring = true;
            _logger.LogInfo($"Shell ViewModel initialized with {RefreshInterval}s refresh interval");

            // Initial update
            UpdateMetrics();
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to initialize monitoring", ex);
            StatusText = "Monitoring initialization failed";
        }
    }

    private void UpdateMetrics()
    {
        try
        {
            var metrics = _monitorService.GetMetrics();
            if (metrics != null)
            {
                SystemMetrics = metrics;
                StatusText = $"Updated: {DateTime.Now:HH:mm:ss}";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to update metrics: {ex.Message}", ex);
            StatusText = "Error updating metrics";
        }
    }

    [RelayCommand]
    public void ChangeRefreshInterval(int seconds)
    {
        if (seconds < 1 || seconds > 60)
            return;

        RefreshInterval = seconds;
        
        if (_updateTimer != null)
        {
            _updateTimer.Stop();
            _updateTimer.Interval = TimeSpan.FromSeconds(seconds);
            _updateTimer.Start();
        }

        _ = _settingsService.SetRefreshIntervalAsync(seconds);
        _logger.LogInfo($"Refresh interval changed to {seconds}s");
    }

    [RelayCommand]
    public void Shutdown()
    {
        _updateTimer?.Stop();
        IsMonitoring = false;
        _logger.LogInfo("Application shutting down");
    }

    [RelayCommand]
    public void RefreshNow()
    {
        UpdateMetrics();
        _logger.LogInfo("Manual refresh triggered");
    }
}
