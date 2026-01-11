using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using SystemPulse.App.Helpers;
using SystemPulse.App.Services;

namespace SystemPulse.App.ViewModels;

public class StartupAppInfo
{
    public string Name { get; set; }
    public string Path { get; set; }
    public int Delay { get; set; }
    public string Source { get; set; } // "Windows", "User", "Manufacturer"
    public bool IsEnabled { get; set; }
}

public partial class StartupAppsViewModel : ObservableObject
{
    private readonly ILoggingService _logger;
    private List<StartupAppInfo> _allApps = new();

    [ObservableProperty]
    private ObservableCollection<StartupAppInfo> startupApps = new();

    [ObservableProperty]
    private string searchQuery = string.Empty;

    [ObservableProperty]
    private string sourceFilter = "All";

    [ObservableProperty]
    private string statusText = "Loading startup apps...";

    [ObservableProperty]
    private bool isLoading = false;

    public List<string> SourceOptions { get; } = new() { "All", "Windows", "User", "Manufacturer" };

    public StartupAppsViewModel(ILoggingService logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [RelayCommand]
    public void LoadStartupApps()
    {
        try
        {
            IsLoading = true;
            StatusText = "Loading startup applications...";

            // Load from registry: HKLM\Software\Microsoft\Windows\CurrentVersion\Run
            // and HKCU\Software\Microsoft\Windows\CurrentVersion\Run
            _allApps = LoadStartupAppsFromRegistry();
            ApplyFiltersAndSort();

            StatusText = $"Loaded {StartupApps.Count} startup applications";
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to load startup apps", ex);
            StatusText = "Error loading startup applications";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public void FilterBySource(string source)
    {
        SourceFilter = source;
        ApplyFiltersAndSort();
    }

    [RelayCommand]
    public void SearchApps()
    {
        ApplyFiltersAndSort();
    }

    [RelayCommand]
    public void ToggleApp(StartupAppInfo app)
    {
        if (app == null)
            return;

        try
        {
            app.IsEnabled = !app.IsEnabled;
            // TODO: Implement registry write to enable/disable startup app
            _logger.LogInfo($"Toggled startup app: {app.Name}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to toggle startup app {app.Name}", ex);
            StatusText = "Error toggling startup app";
        }
    }

    [RelayCommand]
    public void EnableAll()
    {
        foreach (var app in StartupApps)
        {
            app.IsEnabled = true;
        }
        _logger.LogInfo("Enabled all startup applications");
    }

    [RelayCommand]
    public void DisableAll()
    {
        foreach (var app in StartupApps)
        {
            app.IsEnabled = false;
        }
        _logger.LogInfo("Disabled all startup applications");
    }

    private List<StartupAppInfo> LoadStartupAppsFromRegistry()
    {
        var apps = new List<StartupAppInfo>();

        try
        {
            // Load from HKCU (User registry)
            var userKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Run");

            if (userKey != null)
            {
                foreach (var valueName in userKey.GetValueNames())
                {
                    var value = userKey.GetValue(valueName)?.ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        apps.Add(new StartupAppInfo
                        {
                            Name = valueName,
                            Path = value,
                            Delay = 0,
                            Source = "User",
                            IsEnabled = true
                        });
                    }
                }
                userKey.Close();
            }

            // Load from HKLM (System registry)
            var localMachineKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Run");

            if (localMachineKey != null)
            {
                foreach (var valueName in localMachineKey.GetValueNames())
                {
                    var value = localMachineKey.GetValue(valueName)?.ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        apps.Add(new StartupAppInfo
                        {
                            Name = valueName,
                            Path = value,
                            Delay = 0,
                            Source = "Windows",
                            IsEnabled = true
                        });
                    }
                }
                localMachineKey.Close();
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Failed to load startup apps from registry", ex);
        }

        return apps.OrderBy(a => a.Name).ToList();
    }

    private void ApplyFiltersAndSort()
    {
        var filtered = _allApps.AsEnumerable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(SearchQuery))
        {
            filtered = filtered.Where(a =>
                a.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                a.Path.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase));
        }

        // Apply source filter
        if (SourceFilter != "All")
        {
            filtered = filtered.Where(a => a.Source == SourceFilter);
        }

        // Sort by name
        var sorted = filtered.OrderBy(a => a.Name);

        StartupApps.Clear();
        foreach (var app in sorted)
            StartupApps.Add(app);
    }
}
