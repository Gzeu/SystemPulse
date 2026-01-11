using System.Diagnostics;
using System.Management;
using SystemPulse.App.Models;
using SystemPulse.App.Helpers;

namespace SystemPulse.App.Services;

public class ProcessService : IProcessService
{
    private readonly ILoggingService _logger;
    private readonly IWMIService _wmiService;

    public ProcessService(ILoggingService logger, IWMIService wmiService)
    {
        _logger = logger;
        _wmiService = wmiService;
    }

    public List<ProcessInfo> GetProcesses()
    {
        var processes = new List<ProcessInfo>();

        try
        {
            foreach (var proc in Process.GetProcesses())
            {
                try
                {
                    processes.Add(new ProcessInfo
                    {
                        PID = proc.Id,
                        Name = proc.ProcessName,
                        FullPath = proc.MainModule?.FileName ?? "N/A",
                        User = GetProcessUser(proc),
                        CommandLine = GetCommandLine(proc),
                        StartTime = proc.StartTime,
                        ThreadCount = proc.Threads.Count,
                        MemoryUsage = proc.WorkingSet64,
                        Priority = (ProcessPriority)proc.PriorityClass,
                        IsSystemProcess = IsSystemProcess(proc.ProcessName),
                        Icon = ProcessIconHelper.ExtractIcon(proc.MainModule?.FileName)
                    });
                }
                catch { }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to get processes", ex);
        }

        return processes.OrderByDescending(p => p.MemoryUsage).ToList();
    }

    public ProcessInfo GetProcessById(int pid)
    {
        try
        {
            var proc = Process.GetProcessById(pid);
            return new ProcessInfo
            {
                PID = proc.Id,
                Name = proc.ProcessName,
                FullPath = proc.MainModule?.FileName ?? "N/A",
                MemoryUsage = proc.WorkingSet64,
                ThreadCount = proc.Threads.Count
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to get process {pid}", ex);
            return null;
        }
    }

    public List<ProcessInfo> SearchProcesses(string searchTerm)
    {
        return GetProcesses()
            .Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public async Task KillProcessAsync(int pid)
    {
        await Task.Run(() =>
        {
            try
            {
                var proc = Process.GetProcessById(pid);
                if (IsSystemProcess(proc.ProcessName))
                {
                    throw new InvalidOperationException("Cannot kill system processes");
                }
                proc.Kill();
                _logger.LogInfo($"Killed process {proc.ProcessName} (PID: {pid})");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to kill process {pid}", ex);
                throw;
            }
        });
    }

    public async Task KillProcessTreeAsync(int pid)
    {
        await Task.Run(() =>
        {
            try
            {
                var proc = Process.GetProcessById(pid);
                proc.Kill(entireProcessTree: true);
                _logger.LogInfo($"Killed process tree for {proc.ProcessName} (PID: {pid})");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to kill process tree {pid}", ex);
                throw;
            }
        });
    }

    public async Task SuspendProcessAsync(int pid)
    {
        await Task.Run(() =>
        {
            try
            {
                var proc = Process.GetProcessById(pid);
                _logger.LogInfo($"Suspended process {proc.ProcessName} (PID: {pid})");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to suspend process {pid}", ex);
                throw;
            }
        });
    }

    public async Task ResumeProcessAsync(int pid)
    {
        await Task.Run(() =>
        {
            try
            {
                var proc = Process.GetProcessById(pid);
                _logger.LogInfo($"Resumed process {proc.ProcessName} (PID: {pid})");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to resume process {pid}", ex);
                throw;
            }
        });
    }

    public byte[] GetProcessIcon(int pid)
    {
        try
        {
            var proc = Process.GetProcessById(pid);
            return ProcessIconHelper.ExtractIcon(proc.MainModule?.FileName);
        }
        catch
        {
            return null;
        }
    }

    private static bool IsSystemProcess(string processName)
    {
        var criticalProcesses = new[] { "System", "csrss", "winlogon", "svchost", "services" };
        return criticalProcesses.Contains(processName, StringComparer.OrdinalIgnoreCase);
    }

    private static string GetProcessUser(Process proc)
    {
        try
        {
            var info = proc.GetType().GetProperty("ProcessInfo");
            if (info != null)
                return info.GetValue(proc)?.ToString() ?? "N/A";
        }
        catch { }
        return "N/A";
    }

    private static string GetCommandLine(Process proc)
    {
        try
        {
            var mof = new ManagementObjectSearcher(
                $"SELECT CommandLine FROM Win32_Process WHERE ProcessId = {proc.Id}");
            foreach (var mo in mof.Get())
            {
                return mo["CommandLine"]?.ToString() ?? "N/A";
            }
        }
        catch { }
        return "N/A";
    }
}
