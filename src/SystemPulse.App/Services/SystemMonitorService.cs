using SystemPulse.App.Models;
using System.Diagnostics;
using System.Management;

namespace SystemPulse.App.Services;

public interface ISystemMonitorService
{
    Task<SystemMetrics> GetSystemMetricsAsync();
    Task<List<ProcessInfo>> GetProcessesAsync();
    Task KillProcessAsync(int pid);
    Task SuspendProcessAsync(int pid);
    Task ResumeProcessAsync(int pid);
    Task SetProcessPriorityAsync(int pid, ProcessPriorityClass priority);
}

public class SystemMonitorService : ISystemMonitorService
{
    private readonly PerformanceCounter _cpuCounter;
    private readonly PerformanceCounter _ramCounter;
    private PerformanceCounter? _gpuCounter;
    private float _lastCpuValue = 0f;

    public SystemMonitorService()
    {
        _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        _ramCounter = new PerformanceCounter("Memory", "% Committed Bytes In Use");

        // Try to initialize GPU counter (may not be available)
        try
        {
            // Attempt to get GPU usage via performance counter
            // This may not work on all systems
            _gpuCounter = new PerformanceCounter("GPU Engine", "Utilization Percentage", "_Total");
        }
        catch
        {
            // GPU counter not available
            _gpuCounter = null;
        }

        // Initial read to initialize counters
        _cpuCounter.NextValue();
        _ramCounter.NextValue();
        _gpuCounter?.NextValue();
    }

    public async Task<SystemMetrics> GetSystemMetricsAsync()
    {
        return await Task.Run(() =>
        {
            // CPU Usage
            _lastCpuValue = _cpuCounter.NextValue();

            // RAM Usage
            var ramUsage = _ramCounter.NextValue();

            // GPU Usage (placeholder if not available)
            float gpuUsage = 0f;
            if (_gpuCounter != null)
            {
                try
                {
                    gpuUsage = _gpuCounter.NextValue();
                }
                catch
                {
                    // GPU counter failed, return 0
                    gpuUsage = 0f;
                }
            }

            // Network (simplified - bytes sent/received per second)
            var networkSent = GetNetworkBytesSent();
            var networkReceived = GetNetworkBytesReceived();

            // Disk (simplified - read/write per second)
            var diskRead = GetDiskReadBytesPerSec();
            var diskWrite = GetDiskWriteBytesPerSec();

            return new SystemMetrics
            {
                CPUUsage = _lastCpuValue,
                RAMUsage = ramUsage,
                GPUUsage = gpuUsage,
                NetworkSent = networkSent,
                NetworkReceived = networkReceived,
                DiskRead = diskRead,
                DiskWrite = diskWrite,
                Timestamp = DateTime.Now
            };
        });
    }

    public async Task<List<ProcessInfo>> GetProcessesAsync()
    {
        return await Task.Run(() =>
        {
            var processes = Process.GetProcesses();
            var processInfoList = new List<ProcessInfo>();

            foreach (var process in processes)
            {
                try
                {
                    var cpuTime = process.TotalProcessorTime;
                    var cpuUsage = 0.0; // Simplified - actual calculation requires sampling

                    processInfoList.Add(new ProcessInfo
                    {
                        Name = process.ProcessName,
                        PID = process.Id,
                        MemoryUsage = process.WorkingSet64,
                        CPUUsage = cpuUsage,
                        ThreadCount = process.Threads.Count,
                        Status = process.Responding ? "Running" : "Not Responding",
                        Username = GetProcessOwner(process.Id),
                        Priority = process.BasePriority.ToString(),
                        Path = GetProcessPath(process)
                    });
                }
                catch
                {
                    // Skip processes we can't access
                }
            }

            return processInfoList.OrderByDescending(p => p.MemoryUsage).ToList();
        });
    }

    public async Task KillProcessAsync(int pid)
    {
        await Task.Run(() =>
        {
            try
            {
                var process = Process.GetProcessById(pid);
                process.Kill();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to kill process {pid}", ex);
            }
        });
    }

    public async Task SuspendProcessAsync(int pid)
    {
        await Task.Run(() =>
        {
            // Process suspension requires Win32 API (not implemented in placeholder)
            throw new NotImplementedException("Process suspension requires Win32 API integration");
        });
    }

    public async Task ResumeProcessAsync(int pid)
    {
        await Task.Run(() =>
        {
            // Process resumption requires Win32 API (not implemented in placeholder)
            throw new NotImplementedException("Process resumption requires Win32 API integration");
        });
    }

    public async Task SetProcessPriorityAsync(int pid, ProcessPriorityClass priority)
    {
        await Task.Run(() =>
        {
            try
            {
                var process = Process.GetProcessById(pid);
                process.PriorityClass = priority;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to set priority for process {pid}", ex);
            }
        });
    }

    private long GetNetworkBytesSent()
    {
        try
        {
            using var counter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", "*");
            return (long)counter.NextValue();
        }
        catch
        {
            return 0;
        }
    }

    private long GetNetworkBytesReceived()
    {
        try
        {
            using var counter = new PerformanceCounter("Network Interface", "Bytes Received/sec", "*");
            return (long)counter.NextValue();
        }
        catch
        {
            return 0;
        }
    }

    private long GetDiskReadBytesPerSec()
    {
        try
        {
            using var counter = new PerformanceCounter("PhysicalDisk", "Disk Read Bytes/sec", "_Total");
            return (long)counter.NextValue();
        }
        catch
        {
            return 0;
        }
    }

    private long GetDiskWriteBytesPerSec()
    {
        try
        {
            using var counter = new PerformanceCounter("PhysicalDisk", "Disk Write Bytes/sec", "_Total");
            return (long)counter.NextValue();
        }
        catch
        {
            return 0;
        }
    }

    private string GetProcessOwner(int pid)
    {
        try
        {
            var query = $"SELECT * FROM Win32_Process WHERE ProcessId = {pid}";
            using var searcher = new ManagementObjectSearcher(query);
            foreach (ManagementObject obj in searcher.Get())
            {
                var argList = new string[] { string.Empty, string.Empty };
                var returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                if (returnVal == 0)
                {
                    return argList[1] + "\\" + argList[0];
                }
            }
        }
        catch
        {
            // Ignore
        }
        return "Unknown";
    }

    private string GetProcessPath(Process process)
    {
        try
        {
            return process.MainModule?.FileName ?? "Unknown";
        }
        catch
        {
            return "Access Denied";
        }
    }
}
