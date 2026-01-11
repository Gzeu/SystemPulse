using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SystemPulse.App.Models;
using SystemPulse.App.Services;
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml;

namespace SystemPulse.App.ViewModels;

public partial class OverviewViewModel : ObservableObject
{
    private readonly ISystemMonitorService _monitorService;
    private readonly ILoggingService _logger;
    private readonly int _maxHistoryPoints = 60;
    private DispatcherTimer _updateTimer;

    [ObservableProperty]
    private PerformanceMetrics currentMetrics;

    [ObservableProperty]
    private ObservableCollection<float> cpuHistory = new();

    [ObservableProperty]
    private ObservableCollection<float> ramHistory = new();

    [ObservableProperty]
    private ObservableCollection<float> gpuHistory = new();

    [ObservableProperty]
    private string statusText = "Initializing...";

    [ObservableProperty]
    private bool isMonitoring = false;

    public OverviewViewModel(ISystemMonitorService monitorService, ILoggingService logger)
    {
        _monitorService = monitorService ?? throw new ArgumentNullException(nameof(monitorService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        // Initialize with default metrics
        CurrentMetrics = new PerformanceMetrics
        {
            Timestamp = DateTime.Now
        };
    }

    public void StartMonitoring()
    {
        if (IsMonitoring)
            return;

        try
        {
            _updateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _updateTimer.Tick += OnUpdateTick;
            _updateTimer.Start();

            IsMonitoring = true;
            StatusText = "Monitoring active";
            _logger.LogInfo("OverviewViewModel monitoring started");
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to start monitoring", ex);
            StatusText = "Error starting monitoring";
        }
    }

    public void StopMonitoring()
    {
        if (!IsMonitoring)
            return;

        _updateTimer?.Stop();
        IsMonitoring = false;
        StatusText = "Monitoring stopped";
        _logger.LogInfo("OverviewViewModel monitoring stopped");
    }

    private void OnUpdateTick(object sender, object e)
    {
        UpdateMetrics();
    }

    [RelayCommand]
    public void UpdateMetrics()
    {
        try
        {
            var metrics = _monitorService.GetMetrics();
            if (metrics != null)
            {
                CurrentMetrics = metrics;

                // Update history
                AddToHistory(CpuHistory, metrics.CPUUsage);
                AddToHistory(RamHistory, metrics.RAMUsagePercent);
                AddToHistory(GpuHistory, metrics.GPUUsage);

                StatusText = $"Updated: {DateTime.Now:HH:mm:ss}";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to update metrics", ex);
            StatusText = "Error updating metrics";
        }
    }

    public void UpdateMetrics(PerformanceMetrics metrics)
    {
        if (metrics == null)
            return;

        CurrentMetrics = metrics;

        // Update history
        AddToHistory(CpuHistory, metrics.CPUUsage);
        AddToHistory(RamHistory, metrics.RAMUsagePercent);
        AddToHistory(GpuHistory, metrics.GPUUsage);
    }

    private void AddToHistory(ObservableCollection<float> history, float value)
    {
        history.Add(value);

        // Trim history to max points
        if (history.Count > _maxHistoryPoints)
        {
            history.RemoveAt(0);
        }
    }

    [RelayCommand]
    public void RefreshNow()
    {
        UpdateMetrics();
        _logger.LogInfo("Manual refresh triggered");
    }
}
