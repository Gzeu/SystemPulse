# 🎨 Technical Architecture

## Overview

**SystemPulse** is built using a modern, scalable architecture with clear separation of concerns and MVVM pattern.

```
┌─────────────────────────────────────┐
│   WinUI 3 Presentation Layer        │
│  (XAML Views + Controls + Animations)│
└────────────┬────────────────────────┘
             │
             ▼
┌─────────────────────────────────────┐
│   MVVM ViewModel Layer              │
│  (State Management + Commands)      │
└────────────┬────────────────────────┘
             │
             ▼
┌─────────────────────────────────────┐
│   Business Logic / Services         │
│  (Monitoring, Processing, Logging)  │
└────────────┬────────────────────────┘
             │
             ▼
┌─────────────────────────────────────┐
│   System APIs Layer                 │
│  (WMI, Performance Counters, Win32) │
└─────────────────────────────────────┘
```

---

## Architecture Layers

### 1. Presentation Layer (WinUI 3)

**Location:** `Views/`, `Resources/`

**Components:**
- **XAML Pages** - `OverviewPage.xaml`, `ProcessesPage.xaml`, etc.
- **Controls** - Custom WinUI controls, data grids, charts
- **Animations** - Fluent Design animations and transitions
- **Resources** - Colors, styles, typography

**Responsibilities:**
- Display user interface
- Handle user input (clicks, typing)
- Bind to ViewModels
- Apply theming and styling

**Example:**
```xml
<Page x:Class="SystemPulse.App.Views.ProcessesPage">
    <DataGrid 
        ItemsSource="{Binding Processes}"
        SelectionChanged="OnProcessSelected">
        <!-- Columns bound to process properties -->
    </DataGrid>
</Page>
```

---

### 2. ViewModel Layer (MVVM)

**Location:** `ViewModels/`

**Components:**
- **ViewModels** - `ProcessesViewModel`, `OverviewViewModel`, etc.
- **Commands** - `ICommand` implementations for user actions
- **Binding helpers** - Observable collections, property change notification

**Framework:** MVVM Toolkit

**Responsibilities:**
- Manage view state
- Execute commands from UI
- Expose data via `ObservableCollection`
- Handle property change notifications

**Example:**
```csharp
public partial class ProcessesViewModel : ObservableObject
{
    private ObservableCollection<ProcessInfo> _processes;
    
    [ObservableProperty]
    private string searchQuery;

    public ProcessesViewModel(IProcessService processService)
    {
        _processes = new();
        _processService = processService;
    }

    [RelayCommand]
    private async Task KillProcessAsync(ProcessInfo process)
    {
        await _processService.KillProcessAsync(process.PID);
    }
}
```

---

### 3. Business Logic / Services Layer

**Location:** `Services/`

**Core Services:**

#### a. **SystemMonitorService**
- Retrieves real-time performance metrics
- CPU, RAM, GPU, Disk, Network usage
- Handles performance counter aggregation
- Updates UI at 1-2 second intervals

```csharp
public class SystemMonitorService : ISystemMonitorService
{
    public PerformanceMetrics GetMetrics()
    {
        var cpu = _cpuCounter.NextValue();
        var ram = _ramCounter.NextValue();
        return new PerformanceMetrics { CPU = cpu, RAM = ram };
    }
}
```

#### b. **ProcessService**
- Enumerate running processes
- Retrieve process details (CPU%, RAM, Disk I/O)
- Kill, suspend, resume processes
- Extract process icons

```csharp
public class ProcessService : IProcessService
{
    public List<ProcessInfo> GetProcesses()
    {
        return Process.GetProcesses()
            .Select(p => new ProcessInfo { ... })
            .ToList();
    }
}
```

#### c. **WMIService**
- Query Windows Management Instrumentation
- GPU usage detection
- System information retrieval
- Service management

#### d. **PerformanceCounterService**
- Manage performance counters
- Cache counter instances
- Handle counter exceptions gracefully

#### e. **SettingsService**
- Persist user preferences
- Theme settings (Light/Dark/System)
- Refresh rate, opacity, window position

#### f. **LoggingService**
- Structured logging with Serilog
- File and debug output
- Event snapshots

---

### 4. System APIs Layer

**Technologies:**
- **Performance Counters** - `System.Diagnostics.PerformanceCounter`
- **WMI** - `System.Management`
- **Process APIs** - `System.Diagnostics.Process`
- **WinRT APIs** - `Windows.System.Diagnostics`

**Responsibilities:**
- Access Windows system information
- Monitor performance metrics
- Manage processes and services
- Handle platform-specific operations

---

## Data Models

**Location:** `Models/`

### ProcessInfo
```csharp
public class ProcessInfo
{
    public int PID { get; set; }
    public string Name { get; set; }
    public float CPUUsage { get; set; }
    public long MemoryUsage { get; set; }
    public long DiskIO { get; set; }
    public float GPUUsage { get; set; }
    public string User { get; set; }
    public string CommandLine { get; set; }
    public DateTime StartTime { get; set; }
    public byte[] Icon { get; set; }
    public ProcessPriority Priority { get; set; }
}
```

### PerformanceMetrics
```csharp
public class PerformanceMetrics
{
    public float CPUUsage { get; set; }
    public float RAMUsage { get; set; }
    public float GPUUsage { get; set; }
    public float DiskUsage { get; set; }
    public float NetworkUsage { get; set; }
    public DateTime Timestamp { get; set; }
}
```

---

## Design Patterns

### 1. MVVM Pattern
- **Model** - `ProcessInfo`, `PerformanceMetrics` (data models)
- **View** - XAML pages and controls
- **ViewModel** - Data binding and command handling

### 2. Dependency Injection
```csharp
var services = new ServiceCollection();
services.AddSingleton<IProcessService, ProcessService>();
services.AddSingleton<ISystemMonitorService, SystemMonitorService>();
services.AddSingleton<ProcessesViewModel>();
```

### 3. Service Locator
- Centralized service access
- Easy testing and mocking
- Loose coupling

### 4. Observer Pattern
- `ObservableCollection` for data binding
- `INotifyPropertyChanged` for property updates
- Event subscriptions for real-time updates

### 5. Command Pattern
- `RelayCommand` for button clicks
- `AsyncRelayCommand` for async operations
- Type-safe command execution

---

## Threading Model

### DispatcherTimer for UI Updates
```csharp
private DispatcherTimer _updateTimer;

public void StartMonitoring()
{
    _updateTimer = new DispatcherTimer();
    _updateTimer.Interval = TimeSpan.FromSeconds(1);
    _updateTimer.Tick += OnUpdateTick;
    _updateTimer.Start();
}

private void OnUpdateTick(object sender, object e)
{
    var metrics = _monitorService.GetMetrics();
    UpdateUI(metrics);
}
```

### Background Tasks
- Long-running operations use `Task`
- UI updates always on `DispatcherQueue`
- Async/await for clean code flow

---

## Resource Management

### Performance Counters
- Cached and reused
- Exceptions handled gracefully
- Disposed properly on exit

### Memory Management
- Process info updated incrementally
- Old data discarded after use
- Large collections paginated

### Graphics Resources
- Charts use efficient rendering (SkiaSharp)
- Icons cached in memory
- Animation performance optimized

---

## Security Considerations

### Process Protection
- Critical processes marked and protected
- Confirmation dialogs for dangerous operations
- Admin privileges checked for system operations

### WMI Access
- Exception handling for access denied
- Graceful degradation if APIs unavailable
- User permissions respected

### Data Privacy
- No sensitive data logged
- User information handled carefully
- Command lines sanitized

---

## Configuration & Customization

### App Settings
```json
{
  "theme": "System",
  "refreshRate": 2,
  "alwaysOnTop": false,
  "opacity": 1.0,
  "defaultSort": "cpu_desc"
}
```

### Theme System
- Light/Dark/System modes
- Custom color palettes
- Mica/Acrylic effects
- Dynamic accent colors

---

## Error Handling

### Try-Catch Strategy
```csharp
try
{
    var processes = GetProcesses();
}
catch (UnauthorizedAccessException)
{
    // Log and show friendly message
    _logger.LogWarning("Access denied to process list");
}
catch (Exception ex)
{
    // Log critical errors
    _logger.LogError(ex, "Failed to retrieve processes");
}
```

### Logging
- Serilog structured logging
- Different log levels (Debug, Info, Warning, Error)
- File and console sinks

---

## Future Extensions

### Planned Improvements
1. **Plugin System** - Third-party extensions
2. **Custom Dashboards** - User-defined layouts
3. **Remote Monitoring** - Monitor other machines
4. **Analytics** - Historical performance tracking
5. **Notifications** - Alert on resource spikes

---

For API reference, see [API_REFERENCE.md](API_REFERENCE.md)
