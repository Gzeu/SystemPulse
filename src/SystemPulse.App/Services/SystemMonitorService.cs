using System.Diagnostics;
using SystemPulse.App.Models;

namespace SystemPulse.App.Services;

public class SystemMonitorService : ISystemMonitorService
{
    private PerformanceCounter _cpuCounter;
    private PerformanceCounter _ramCounter;
    private PerformanceCounter _networkCounter;
    private readonly ILoggingService _logger;
    private DispatcherTimer _updateTimer;

    public event EventHandler<PerformanceMetrics> MetricsUpdated;

    public SystemMonitorService(ILoggingService logger)
    {
        _logger = logger;
        InitializeCounters();
    }

    private void InitializeCounters()
    {
        try
        {
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total", true);
            _ramCounter = new PerformanceCounter("Memory", "% Committed Bytes In Use", "", true);
            _networkCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", null, true);

            _cpuCounter.NextValue();
            System.Threading.Thread.Sleep(100);

            _logger.LogInfo("Performance counters initialized successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to initialize performance counters", ex);
        }
    }

    public PerformanceMetrics GetMetrics()
    {
        try
        {
            var totalMemory = GC.GetTotalMemory(false);
            var availableMemory = GetAvailableMemory();
            var usedMemory = totalMemory - availableMemory;

            return new PerformanceMetrics
            {
                CPUUsage = GetCPUUsage(),
                RAMUsagePercent = GetRAMUsagePercent(),
                RAMUsageBytes = usedMemory,
                TotalRAMBytes = totalMemory,
                GPUUsage = GetGPUUsage(),
                NetworkUsageMbps = GetNetworkUsage(),
                ProcessCount = Process.GetProcesses().Length,
                ThreadCount = Process.GetCurrentProcess().Threads.Count,
                Timestamp = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("Error getting metrics", ex);
            return new PerformanceMetrics { Timestamp = DateTime.Now };
        }
    }

    public float GetCPUUsage()
    {
        try
        {
            return Math.Min(100, _cpuCounter?.NextValue() ?? 0);
        }
        catch { return 0; }
    }

    public float GetRAMUsagePercent()
    {
        try
        {
            return _ramCounter?.NextValue() ?? 0;
        }
        catch { return 0; }
    }

    public long GetRAMUsageBytes()
    {
        return GC.GetTotalMemory(false);
    }

    public long GetTotalRAMBytes()
    {
        var info = new Microsoft.VisualBasic.Devices.ComputerInfo();
        return (long)info.TotalPhysicalMemory;
    }

    public float GetGPUUsage()
    {
        return 0; // TODO: Implement WMI-based GPU detection
    }

    public float GetNetworkUsage()
    {
        try
        {
            return (_networkCounter?.NextValue() ?? 0) / 1024 / 1024;
        }
        catch { return 0; }
    }

    public void StartMonitoring()
    {
        _updateTimer ??= new DispatcherTimer();
        _updateTimer.Interval = TimeSpan.FromSeconds(1);
        _updateTimer.Tick += (s, e) => MetricsUpdated?.Invoke(this, GetMetrics());
        _updateTimer.Start();
    }

    public void StopMonitoring()
    {
        _updateTimer?.Stop();
    }

    private static long GetAvailableMemory()
    {
        try
        {
            var info = new Microsoft.VisualBasic.Devices.ComputerInfo();
            return (long)info.AvailablePhysicalMemory;
        }
        catch
        {
            return 0;
        }
    }
}
