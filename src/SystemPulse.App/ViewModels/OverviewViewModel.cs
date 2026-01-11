using CommunityToolkit.Mvvm.ComponentModel;
using SystemPulse.App.Models;
using SystemPulse.App.Services;
using System.Collections.ObjectModel;

namespace SystemPulse.App.ViewModels;

public partial class OverviewViewModel : ObservableObject
{
    private readonly ISystemMonitorService _monitorService;
    private readonly ILoggingService _logger;
    private readonly int _maxHistoryPoints = 60;

    [ObservableProperty]
    private PerformanceMetrics currentMetrics;

    [ObservableProperty]
    private ObservableCollection<float> cpuHistory = new();

    [ObservableProperty]
    private ObservableCollection<float> ramHistory = new();

    [ObservableProperty]
    private ObservableCollection<float> gpuHistory = new();

    public OverviewViewModel(ISystemMonitorService monitorService, ILoggingService logger)
    {
        _monitorService = monitorService ?? throw new ArgumentNullException(nameof(monitorService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void UpdateMetrics(PerformanceMetrics metrics)
    {
        CurrentMetrics = metrics;

        // Update history
        CpuHistory.Add(metrics.CPUUsage);
        RamHistory.Add(metrics.RAMUsagePercent);
        GpuHistory.Add(metrics.GPUUsage);

        // Trim history
        if (CpuHistory.Count > _maxHistoryPoints)
            CpuHistory.RemoveAt(0);
        if (RamHistory.Count > _maxHistoryPoints)
            RamHistory.RemoveAt(0);
        if (GpuHistory.Count > _maxHistoryPoints)
            GpuHistory.RemoveAt(0);
    }
}
