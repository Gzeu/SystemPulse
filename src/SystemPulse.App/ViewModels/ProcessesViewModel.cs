using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SystemPulse.App.Models;
using SystemPulse.App.Services;
using System.Collections.ObjectModel;

namespace SystemPulse.App.ViewModels;

public partial class ProcessesViewModel : ObservableObject
{
    private readonly IProcessService _processService;
    private readonly ILoggingService _logger;
    private List<ProcessInfo> _allProcesses = new();

    [ObservableProperty]
    private ObservableCollection<ProcessInfo> processes = new();

    [ObservableProperty]
    private string searchQuery = string.Empty;

    [ObservableProperty]
    private string sortColumn = "Memory";

    [ObservableProperty]
    private bool sortDescending = true;

    [ObservableProperty]
    private ProcessInfo selectedProcess;

    public ProcessesViewModel(IProcessService processService, ILoggingService logger)
    {
        _processService = processService ?? throw new ArgumentNullException(nameof(processService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [RelayCommand]
    public void RefreshProcesses()
    {
        try
        {
            _allProcesses = _processService.GetProcesses();
            ApplyFiltersAndSort();
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to refresh processes", ex);
        }
    }

    [RelayCommand]
    public void SearchProcesses()
    {
        ApplyFiltersAndSort();
    }

    [RelayCommand]
    public void SortByColumn(string columnName)
    {
        if (SortColumn == columnName)
            SortDescending = !SortDescending;
        else
            SortColumn = columnName;

        ApplyFiltersAndSort();
    }

    [RelayCommand]
    public async Task KillProcessAsync(ProcessInfo process)
    {
        if (process == null) return;

        try
        {
            await _processService.KillProcessAsync(process.PID);
            _logger.LogInfo($"Killed process: {process.Name}");
            RefreshProcesses();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to kill process {process.Name}", ex);
        }
    }

    [RelayCommand]
    public async Task SuspendProcessAsync(ProcessInfo process)
    {
        if (process == null) return;

        try
        {
            await _processService.SuspendProcessAsync(process.PID);
            _logger.LogInfo($"Suspended process: {process.Name}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to suspend process", ex);
        }
    }

    private void ApplyFiltersAndSort()
    {
        var filtered = _allProcesses.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchQuery))
            filtered = filtered.Where(p => p.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase));

        var sorted = SortColumn switch
        {
            "CPU" => SortDescending ? filtered.OrderByDescending(p => p.CPUUsage) : filtered.OrderBy(p => p.CPUUsage),
            "Memory" => SortDescending ? filtered.OrderByDescending(p => p.MemoryUsage) : filtered.OrderBy(p => p.MemoryUsage),
            "Name" => SortDescending ? filtered.OrderByDescending(p => p.Name) : filtered.OrderBy(p => p.Name),
            _ => filtered.OrderByDescending(p => p.MemoryUsage)
        };

        Processes.Clear();
        foreach (var process in sorted)
            Processes.Add(process);
    }
}
