using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SystemPulse.App.Models;
using SystemPulse.App.Services;
using SystemPulse.App.Helpers;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.UI.Xaml;

namespace SystemPulse.App.ViewModels;

public partial class ProcessesViewModel : ObservableObject
{
    private readonly IProcessService _processService;
    private readonly ILoggingService _logger;
    private readonly DialogHelper _dialogHelper;
    private List<ProcessInfo> _allProcesses = new();
    private DispatcherTimer _updateTimer;

    [ObservableProperty]
    private ObservableCollection<ProcessInfo> processes = new();

    [ObservableProperty]
    private ProcessInfo selectedProcess;

    [ObservableProperty]
    private string searchText = string.Empty;

    [ObservableProperty]
    private string statusText = "Ready";

    [ObservableProperty]
    private bool isLoading = false;

    [ObservableProperty]
    private int totalProcessCount;

    [ObservableProperty]
    private int filteredProcessCount;

    public ProcessesViewModel(
        IProcessService processService,
        ILoggingService logger,
        DialogHelper dialogHelper)
    {
        _processService = processService ?? throw new ArgumentNullException(nameof(processService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _dialogHelper = dialogHelper ?? throw new ArgumentNullException(nameof(dialogHelper));
    }

    partial void OnSearchTextChanged(string value)
    {
        FilterProcesses();
    }

    public void StartMonitoring()
    {
        if (_updateTimer != null)
            return;

        _updateTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(2)
        };
        _updateTimer.Tick += (s, e) => LoadProcesses();
        _updateTimer.Start();

        _logger.LogInfo("Process monitoring started");
    }

    public void StopMonitoring()
    {
        _updateTimer?.Stop();
        _updateTimer = null;
        _logger.LogInfo("Process monitoring stopped");
    }

    [RelayCommand]
    public async Task LoadProcessesAsync()
    {
        await Task.Run(() => LoadProcesses());
    }

    private void LoadProcesses()
    {
        try
        {
            IsLoading = true;
            StatusText = "Loading processes...";

            var processes = _processService.GetProcesses();
            _allProcesses = processes.ToList();
            TotalProcessCount = _allProcesses.Count;

            FilterProcesses();

            StatusText = $"Loaded {TotalProcessCount} processes";
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to load processes", ex);
            StatusText = "Error loading processes";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void FilterProcesses()
    {
        IEnumerable<ProcessInfo> filtered = _allProcesses;

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var search = SearchText.ToLowerInvariant();
            filtered = filtered.Where(p =>
                p.Name.ToLowerInvariant().Contains(search) ||
                p.PID.ToString().Contains(search) ||
                (p.Username?.ToLowerInvariant().Contains(search) ?? false));
        }

        // Update UI collection
        Processes.Clear();
        foreach (var process in filtered.OrderByDescending(p => p.CPUUsage).Take(500))
        {
            Processes.Add(process);
        }

        FilteredProcessCount = Processes.Count;
    }

    [RelayCommand]
    public async Task KillProcessAsync()
    {
        if (SelectedProcess == null)
            return;

        try
        {
            var confirm = await _dialogHelper.ShowConfirmDialogAsync(
                "Kill Process",
                $"Are you sure you want to kill '{SelectedProcess.Name}' (PID: {SelectedProcess.PID})?\n\nThis action cannot be undone.");

            if (!confirm)
                return;

            IsLoading = true;
            var success = await _processService.KillProcessAsync(SelectedProcess.PID);

            if (success)
            {
                StatusText = $"Process '{SelectedProcess.Name}' terminated";
                _logger.LogInfo($"Killed process: {SelectedProcess.Name} (PID: {SelectedProcess.PID})");
                await Task.Delay(500);
                LoadProcesses();
            }
            else
            {
                await _dialogHelper.ShowErrorDialogAsync(
                    "Kill Failed",
                    $"Failed to kill process '{SelectedProcess.Name}'. You may need administrator privileges.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error killing process {SelectedProcess?.PID}", ex);
            await _dialogHelper.ShowErrorDialogAsync("Error", $"An error occurred: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public async Task SuspendProcessAsync()
    {
        if (SelectedProcess == null)
            return;

        try
        {
            IsLoading = true;
            var success = await _processService.SuspendProcessAsync(SelectedProcess.PID);

            if (success)
            {
                StatusText = $"Process '{SelectedProcess.Name}' suspended";
                _logger.LogInfo($"Suspended process: {SelectedProcess.Name} (PID: {SelectedProcess.PID})");
            }
            else
            {
                await _dialogHelper.ShowErrorDialogAsync(
                    "Suspend Failed",
                    $"Failed to suspend process '{SelectedProcess.Name}'.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error suspending process {SelectedProcess?.PID}", ex);
            StatusText = "Error suspending process";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public async Task ResumeProcessAsync()
    {
        if (SelectedProcess == null)
            return;

        try
        {
            IsLoading = true;
            var success = await _processService.ResumeProcessAsync(SelectedProcess.PID);

            if (success)
            {
                StatusText = $"Process '{SelectedProcess.Name}' resumed";
                _logger.LogInfo($"Resumed process: {SelectedProcess.Name} (PID: {SelectedProcess.PID})");
            }
            else
            {
                await _dialogHelper.ShowErrorDialogAsync(
                    "Resume Failed",
                    $"Failed to resume process '{SelectedProcess.Name}'.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error resuming process {SelectedProcess?.PID}", ex);
            StatusText = "Error resuming process";
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task SetProcessPriorityAsync(ProcessPriorityClass priority)
    {
        if (SelectedProcess == null)
            return;

        try
        {
            IsLoading = true;
            var success = await _processService.SetProcessPriorityAsync(SelectedProcess.PID, priority);

            if (success)
            {
                StatusText = $"Priority set to {priority} for '{SelectedProcess.Name}'";
                _logger.LogInfo($"Set priority to {priority} for process: {SelectedProcess.Name} (PID: {SelectedProcess.PID})");
                await Task.Delay(500);
                LoadProcesses();
            }
            else
            {
                await _dialogHelper.ShowErrorDialogAsync(
                    "Priority Change Failed",
                    $"Failed to set priority for '{SelectedProcess.Name}'.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error setting priority for process {SelectedProcess?.PID}", ex);
            StatusText = "Error setting priority";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public void ShowProcessDetails()
    {
        if (SelectedProcess == null)
            return;

        // TODO: Navigate to DetailsPage with selected process
        StatusText = $"Details for '{SelectedProcess.Name}' - Feature coming soon";
        _logger.LogInfo($"Process details requested for: {SelectedProcess.Name}");
    }

    [RelayCommand]
    public async Task OpenFileLocationAsync()
    {
        if (SelectedProcess == null || string.IsNullOrWhiteSpace(SelectedProcess.Path))
            return;

        try
        {
            var directory = System.IO.Path.GetDirectoryName(SelectedProcess.Path);
            if (!string.IsNullOrEmpty(directory) && System.IO.Directory.Exists(directory))
            {
                Process.Start("explorer.exe", directory);
                StatusText = $"Opened location for '{SelectedProcess.Name}'";
            }
            else
            {
                await _dialogHelper.ShowErrorDialogAsync(
                    "Location Not Found",
                    $"Cannot find file location for '{SelectedProcess.Name}'.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error opening file location for {SelectedProcess?.Name}", ex);
            StatusText = "Error opening file location";
        }
    }
}
