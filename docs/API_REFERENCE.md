# API Reference

## Overview

This document describes the public APIs and services in **SystemPulse**.

---

## Services

### ISystemMonitorService

Responsible for retrieving real-time system performance metrics.

```csharp
public interface ISystemMonitorService
{
    /// <summary>Gets current system performance metrics</summary>
    PerformanceMetrics GetMetrics();
    
    /// <summary>Gets CPU usage percentage (0-100)</summary>
    float GetCPUUsage();
    
    /// <summary>Gets RAM usage in bytes</summary>
    long GetRAMUsage();
    
    /// <summary>Gets total available RAM in bytes</summary>
    long GetTotalRAM();
    
    /// <summary>Gets GPU usage percentage if available</summary>
    float GetGPUUsage();
    
    /// <summary>Gets disk usage in bytes</summary>
    long GetDiskUsage();
    
    /// <summary>Gets network throughput in bytes</summary>
    long GetNetworkUsage();
}
```

**Example Usage:**
```csharp
var metrics = _monitorService.GetMetrics();
Debug.WriteLine($"CPU: {metrics.CPUUsage}%");
Debug.WriteLine($"RAM: {metrics.RAMUsage / 1024.0 / 1024.0} MB");
```

---

### IProcessService

Manages process enumeration, querying, and control.

```csharp
public interface IProcessService
{
    /// <summary>Gets all running processes</summary>
    List<ProcessInfo> GetProcesses();
    
    /// <summary>Gets process by PID</summary>
    ProcessInfo GetProcessById(int pid);
    
    /// <summary>Filters processes by search term</summary>
    List<ProcessInfo> SearchProcesses(string searchTerm);
    
    /// <summary>Kills a process by PID</summary>
    Task KillProcessAsync(int pid);
    
    /// <summary>Kills process tree (process + children)</summary>
    Task KillProcessTreeAsync(int pid);
    
    /// <summary>Suspends all threads in a process</summary>
    Task SuspendProcessAsync(int pid);
    
    /// <summary>Resumes all threads in a process</summary>
    Task ResumeProcessAsync(int pid);
    
    /// <summary>Gets process icon as byte array</summary>
    byte[] GetProcessIcon(int pid);
}
```

**Example Usage:**
```csharp
var processes = _processService.GetProcesses();
var chrome = _processService.SearchProcesses("chrome").FirstOrDefault();

if (chrome != null)
{
    await _processService.KillProcessAsync(chrome.PID);
}
```

---

### ISettingsService

Handles application settings persistence and retrieval.

```csharp
public interface ISettingsService
{
    /// <summary>Gets setting value by key</summary>
    T GetSetting<T>(string key, T defaultValue = default);
    
    /// <summary>Sets setting value</summary>
    Task SetSettingAsync<T>(string key, T value);
    
    /// <summary>Gets theme preference (Light/Dark/System)</summary>
    string GetTheme();
    
    /// <summary>Sets theme preference</summary>
    Task SetThemeAsync(string theme);
    
    /// <summary>Gets refresh interval in seconds</summary>
    int GetRefreshInterval();
    
    /// <summary>Sets refresh interval</summary>
    Task SetRefreshIntervalAsync(int seconds);
    
    /// <summary>Gets window opacity (0.0-1.0)</summary>
    double GetWindowOpacity();
    
    /// <summary>Sets window opacity</summary>
    Task SetWindowOpacityAsync(double opacity);
}
```

**Example Usage:**
```csharp
var theme = _settingsService.GetTheme();
await _settingsService.SetThemeAsync("Dark");

var refreshRate = _settingsService.GetRefreshInterval();
await _settingsService.SetRefreshIntervalAsync(2);
```

---

### ILoggingService

Provides structured logging capabilities.

```csharp
public interface ILoggingService
{
    /// <summary>Logs debug message</summary>
    void LogDebug(string message);
    
    /// <summary>Logs information message</summary>
    void LogInfo(string message);
    
    /// <summary>Logs warning message</summary>
    void LogWarning(string message, Exception ex = null);
    
    /// <summary>Logs error message</summary>
    void LogError(string message, Exception ex = null);
    
    /// <summary>Creates a snapshot of current state</summary>
    Task<string> ExportSnapshotAsync();
}
```

**Example Usage:**
```csharp
_logger.LogInfo("SystemPulse started");

try
{
    var processes = GetProcesses();
}
catch (Exception ex)
{
    _logger.LogError("Failed to get processes", ex);
}
```

---

## Data Models

### ProcessInfo

Represents a running process.

```csharp
public class ProcessInfo
{
    public int PID { get; set; }
    public string Name { get; set; }
    public string FullPath { get; set; }
    public float CPUUsage { get; set; }  // 0-100%
    public long MemoryUsage { get; set; }  // bytes
    public long DiskIOBytesPerSec { get; set; }
    public float GPUUsage { get; set; }  // 0-100%
    public string User { get; set; }
    public string CommandLine { get; set; }
    public DateTime StartTime { get; set; }
    public ProcessState State { get; set; }
    public ProcessPriority Priority { get; set; }
    public int ThreadCount { get; set; }
    public byte[] Icon { get; set; }
    public bool IsSystemProcess { get; set; }
}

public enum ProcessState
{
    Running = 0,
    Suspended = 1,
    Terminated = 2
}

public enum ProcessPriority
{
    Idle = 0,
    BelowNormal = 1,
    Normal = 2,
    AboveNormal = 3,
    High = 4,
    Realtime = 5
}
```

---

### PerformanceMetrics

Represents system-wide performance data.

```csharp
public class PerformanceMetrics
{
    public float CPUUsage { get; set; }  // 0-100%
    public float RAMUsagePercent { get; set; }  // 0-100%
    public long RAMUsageBytes { get; set; }
    public long TotalRAMBytes { get; set; }
    public float GPUUsage { get; set; }  // 0-100%
    public float DiskUsagePercent { get; set; }  // 0-100%
    public float NetworkUsageMbps { get; set; }
    public int ProcessCount { get; set; }
    public int ThreadCount { get; set; }
    public DateTime Timestamp { get; set; }
}
```

---

### ServiceInfo

Represents a Windows service.

```csharp
public class ServiceInfo
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public ServiceState State { get; set; }
    public ServiceStartMode StartMode { get; set; }
    public string ProcessName { get; set; }
}

public enum ServiceState
{
    Running = 0,
    Stopped = 1,
    StartPending = 2,
    StopPending = 3
}

public enum ServiceStartMode
{
    Boot = 0,
    System = 1,
    Automatic = 2,
    Manual = 3,
    Disabled = 4
}
```

---

## ViewModels

### ProcessesViewModel

Manages process list and filtering.

```csharp
public partial class ProcessesViewModel : ObservableObject
{
    // Properties
    public ObservableCollection<ProcessInfo> Processes { get; }
    
    [ObservableProperty]
    private string searchQuery;
    
    [ObservableProperty]
    private string sortColumn = "CPU";
    
    [ObservableProperty]
    private bool sortDescending = true;
    
    // Commands
    public IAsyncRelayCommand<ProcessInfo> KillProcessCommand { get; }
    public IAsyncRelayCommand<ProcessInfo> SuspendProcessCommand { get; }
    public IAsyncRelayCommand<ProcessInfo> ResumeProcessCommand { get; }
    
    // Methods
    public void RefreshProcesses();
    public void FilterProcesses();
    public void SortByColumn(string columnName);
}
```

---

## Converters

### BytesToGBConverter

Converts bytes to gigabytes.

```csharp
public class BytesToGBConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (long.TryParse(value?.ToString(), out long bytes))
        {
            return (bytes / 1024.0 / 1024.0 / 1024.0).ToString("F2");
        }
        return "0 GB";
    }
}
```

**Usage in XAML:**
```xml
<TextBlock Text="{Binding ProcessMemory, Converter={StaticResource BytesToGBConverter}}"/>
```

---

## Helpers

### ProcessIconHelper

Extracts icons from process executables.

```csharp
public static class ProcessIconHelper
{
    /// <summary>Extracts icon from process executable</summary>
    public static byte[] ExtractIcon(string processPath);
    
    /// <summary>Extracts icon and converts to bitmap</summary>
    public static BitmapImage ExtractIconAsBitmap(string processPath);
}
```

---

## Constants

```csharp
public static class AppConstants
{
    public const int REFRESH_INTERVAL_MS = 2000;
    public const int PROCESS_QUERY_TIMEOUT_MS = 5000;
    public const int WMI_QUERY_TIMEOUT_MS = 3000;
    public const string CRITICAL_SYSTEM_PROCESSES = "System,csrss,winlogon,svchost";
    public const double DEFAULT_WINDOW_OPACITY = 1.0;
}
```

---

## Error Handling

### Common Exceptions

- **UnauthorizedAccessException** - Insufficient permissions
- **ProcessAccessException** - Cannot access process
- **WMIAccessException** - WMI query failed
- **InvalidOperationException** - Invalid operation on process

**Handling Example:**
```csharp
try
{
    await _processService.KillProcessAsync(pid);
}
catch (UnauthorizedAccessException)
{
    _logger.LogWarning("Insufficient privileges to kill process");
}
catch (Exception ex)
{
    _logger.LogError("Failed to kill process", ex);
}
```

---

For more information, see [ARCHITECTURE.md](ARCHITECTURE.md)
