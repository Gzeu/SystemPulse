using SystemPulse.App.Models;

namespace SystemPulse.App.Services;

public interface ISystemMonitorService
{
    PerformanceMetrics GetMetrics();
    float GetCPUUsage();
    float GetRAMUsagePercent();
    long GetRAMUsageBytes();
    long GetTotalRAMBytes();
    float GetGPUUsage();
    float GetNetworkUsage();
    void StartMonitoring();
    void StopMonitoring();
    event EventHandler<PerformanceMetrics> MetricsUpdated;
}
