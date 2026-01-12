using System.Diagnostics;
using System.Management;
using SystemPulse.App.Models;
using SystemPulse.App.Helpers;

namespace SystemPulse.App.Services;

public class SystemMonitorService : ISystemMonitorService
{
    private readonly PerformanceCounter _cpuCounter;
    private readonly PerformanceCounter _ramCounter;
    private float _lastCpuValue = 0f;

    public SystemMonitorService()
    {
        _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        _ramCounter = new PerformanceCounter("Memory", "% Committed Bytes In Use");

        _cpuCounter.NextValue();
        _ramCounter.NextValue();
    }

    public async Task<SystemMetrics> GetSystemMetricsAsync()
    {
        return await Task.Run(() =>
        {
            _lastCpuValue = _cpuCounter.NextValue();
            var ramUsage = _ramCounter.NextValue();
            
            // Use our new DXGI-based GPU helper
            float gpuUsage = GPUHelper.GetGpuUsage();

            return new SystemMetrics
            {
                CPUUsage = _lastCpuValue,
                RAMUsage = ramUsage,
                GPUUsage = gpuUsage,
                NetworkSent = GetNetworkBytesSent(),
                NetworkReceived = GetNetworkBytesReceived(),
                DiskRead = GetDiskReadBytesPerSec(),
                DiskWrite = GetDiskWriteBytesPerSec(),
                Timestamp = DateTime.Now
            };
        });
    }
    
    // ... existing Kill/Suspend/Priority methods ...
    public async Task KillProcessAsync(int pid)
    {
        await Task.Run(() => { Process.GetProcessById(pid).Kill(); });
    }

    public async Task SuspendProcessAsync(int pid)
    {
        await Task.Run(() => { /* Win32 PInvoke needed for thread suspension */ });
    }

    public async Task ResumeProcessAsync(int pid)
    {
        await Task.Run(() => { /* Win32 PInvoke needed for thread resumption */ });
    }

    public async Task SetProcessPriorityAsync(int pid, ProcessPriorityClass priority)
    {
        await Task.Run(() => { Process.GetProcessById(pid).PriorityClass = priority; });
    }

    private long GetNetworkBytesSent() => 0; // Placeholder
    private long GetNetworkBytesReceived() => 0; // Placeholder
    private long GetDiskReadBytesPerSec() => 0; // Placeholder
    private long GetDiskWriteBytesPerSec() => 0; // Placeholder

    public async Task<List<ProcessInfo>> GetProcessesAsync()
    {
        return await Task.Run(() =>
        {
            return Process.GetProcesses().Select(p => new ProcessInfo
            {
                Name = p.ProcessName,
                PID = p.Id,
                MemoryUsage = p.WorkingSet64,
                CPUUsage = 0,
                Status = p.Responding ? "Running" : "Not Responding"
            }).ToList();
        });
    }
}
