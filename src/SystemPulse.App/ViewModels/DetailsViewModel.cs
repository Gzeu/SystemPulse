using CommunityToolkit.Mvvm.ComponentModel;
using SystemPulse.App.Models;
using SystemPulse.App.Services;
using System.Collections.ObjectModel;

namespace SystemPulse.App.ViewModels;

public partial class DetailsViewModel : ObservableObject
{
    private readonly ILoggingService _logger;

    [ObservableProperty]
    private ProcessInfo selectedProcess;

    [ObservableProperty]
    private ObservableCollection<KeyValuePair<string, string>> environmentVariables = new();

    [ObservableProperty]
    private ObservableCollection<string> modules = new();

    [ObservableProperty]
    private int moduleCount;

    public DetailsViewModel(ILoggingService logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        SelectedProcess = new ProcessInfo(); // Default empty
    }

    public void LoadProcessDetails(ProcessInfo process)
    {
        try
        {
            if (process == null)
            {
                _logger.LogWarning("Attempted to load null process details");
                return;
            }

            SelectedProcess = process;

            // Load environment variables
            LoadEnvironmentVariables(process.PID);

            // Load modules
            LoadModules(process.PID);

            _logger.LogInfo($"Loaded details for process {process.Name} (PID: {process.PID})");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to load process details for PID {process?.PID}", ex);
        }
    }

    private void LoadEnvironmentVariables(int pid)
    {
        EnvironmentVariables.Clear();

        try
        {
            var proc = System.Diagnostics.Process.GetProcessById(pid);
            var envVars = proc.StartInfo.EnvironmentVariables;

            foreach (System.Collections.DictionaryEntry entry in envVars)
            {
                EnvironmentVariables.Add(new KeyValuePair<string, string>(
                    entry.Key?.ToString() ?? "Unknown",
                    entry.Value?.ToString() ?? "Unknown"
                ));
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Could not load environment variables for PID {pid}: {ex.Message}");
            EnvironmentVariables.Add(new KeyValuePair<string, string>(
                "Access Denied",
                "Insufficient permissions to access process environment variables"
            ));
        }
    }

    private void LoadModules(int pid)
    {
        Modules.Clear();

        try
        {
            var proc = System.Diagnostics.Process.GetProcessById(pid);
            var modules = proc.Modules;

            foreach (System.Diagnostics.ProcessModule module in modules)
            {
                Modules.Add($"{module.ModuleName} ({module.FileName})");
            }

            ModuleCount = Modules.Count;
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Could not load modules for PID {pid}: {ex.Message}");
            Modules.Add("Access Denied: Insufficient permissions to access process modules");
            ModuleCount = 0;
        }
    }
}
