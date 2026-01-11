using CommunityToolkit.Mvvm.ComponentModel;
using SystemPulse.App.Models;
using System.Collections.ObjectModel;

namespace SystemPulse.App.ViewModels;

public partial class PerformanceViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<float> cpuHistory = new();

    [ObservableProperty]
    private ObservableCollection<float> ramHistory = new();

    [ObservableProperty]
    private ObservableCollection<float> gpuHistory = new();

    [ObservableProperty]
    private ObservableCollection<float> diskHistory = new();

    [ObservableProperty]
    private ObservableCollection<float> networkHistory = new();

    [ObservableProperty]
    private PerformanceMetrics currentMetrics;

    private readonly int _maxHistoryPoints = 300; // 5 minutes at 1 second intervals

    public void UpdateMetrics(PerformanceMetrics metrics)
    {
        CurrentMetrics = metrics;

        AddToHistory(CpuHistory, metrics.CPUUsage);
        AddToHistory(RamHistory, metrics.RAMUsagePercent);
        AddToHistory(GpuHistory, metrics.GPUUsage);
        AddToHistory(DiskHistory, metrics.DiskUsagePercent);
        AddToHistory(NetworkHistory, metrics.NetworkUsageMbps);
    }

    private void AddToHistory(ObservableCollection<float> history, float value)
    {
        history.Add(value);
        if (history.Count > _maxHistoryPoints)
            history.RemoveAt(0);
    }
}
