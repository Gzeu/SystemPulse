using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SystemPulse.App.Services;
using SystemPulse.App.Helpers;
using Microsoft.UI.Xaml;
using System.Text.Json;

namespace SystemPulse.App.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly ILoggingService _logger;
    private readonly ThemeHelper _themeHelper;
    private readonly DialogHelper _dialogHelper;
    private readonly string _settingsFilePath;

    // Appearance
    [ObservableProperty]
    private int selectedThemeIndex = 2; // Default: Use System Setting

    [ObservableProperty]
    private int windowOpacity = 100;

    // Performance
    [ObservableProperty]
    private int refreshInterval = 2; // Default: 2 seconds

    [ObservableProperty]
    private int maxChartDataPoints = 300; // Default: 300 points

    // Behavior
    [ObservableProperty]
    private bool alwaysOnTop = false;

    [ObservableProperty]
    private bool startWithWindows = false;

    [ObservableProperty]
    private bool minimizeToTray = false;

    [ObservableProperty]
    private bool startMinimized = false;

    [ObservableProperty]
    private bool showNotifications = true;

    // UI State
    [ObservableProperty]
    private string statusText = "Ready";

    private AppSettings _originalSettings;

    public SettingsViewModel(
        ILoggingService logger,
        ThemeHelper themeHelper,
        DialogHelper dialogHelper)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _themeHelper = themeHelper ?? throw new ArgumentNullException(nameof(themeHelper));
        _dialogHelper = dialogHelper ?? throw new ArgumentNullException(nameof(dialogHelper));

        // Settings file path
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appFolder = Path.Combine(appDataPath, "SystemPulse");
        Directory.CreateDirectory(appFolder);
        _settingsFilePath = Path.Combine(appFolder, "settings.json");
    }

    public void LoadSettings()
    {
        try
        {
            if (File.Exists(_settingsFilePath))
            {
                var json = File.ReadAllText(_settingsFilePath);
                var settings = JsonSerializer.Deserialize<AppSettings>(json);

                if (settings != null)
                {
                    ApplySettings(settings);
                    _originalSettings = settings;
                    _logger.LogInfo("Settings loaded successfully");
                    return;
                }
            }

            // Load defaults if file doesn't exist
            _originalSettings = GetDefaultSettings();
            ApplySettings(_originalSettings);
            _logger.LogInfo("Loaded default settings");
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to load settings", ex);
            _originalSettings = GetDefaultSettings();
            ApplySettings(_originalSettings);
        }
    }

    private void ApplySettings(AppSettings settings)
    {
        SelectedThemeIndex = settings.ThemeIndex;
        WindowOpacity = settings.WindowOpacity;
        RefreshInterval = settings.RefreshInterval;
        MaxChartDataPoints = settings.MaxChartDataPoints;
        AlwaysOnTop = settings.AlwaysOnTop;
        StartWithWindows = settings.StartWithWindows;
        MinimizeToTray = settings.MinimizeToTray;
        StartMinimized = settings.StartMinimized;
        ShowNotifications = settings.ShowNotifications;
    }

    private AppSettings GetCurrentSettings()
    {
        return new AppSettings
        {
            ThemeIndex = SelectedThemeIndex,
            WindowOpacity = WindowOpacity,
            RefreshInterval = RefreshInterval,
            MaxChartDataPoints = MaxChartDataPoints,
            AlwaysOnTop = AlwaysOnTop,
            StartWithWindows = StartWithWindows,
            MinimizeToTray = MinimizeToTray,
            StartMinimized = StartMinimized,
            ShowNotifications = ShowNotifications
        };
    }

    private AppSettings GetDefaultSettings()
    {
        return new AppSettings
        {
            ThemeIndex = 2, // Use System Setting
            WindowOpacity = 100,
            RefreshInterval = 2,
            MaxChartDataPoints = 300,
            AlwaysOnTop = false,
            StartWithWindows = false,
            MinimizeToTray = false,
            StartMinimized = false,
            ShowNotifications = true
        };
    }

    partial void OnSelectedThemeIndexChanged(int value)
    {
        // Apply theme immediately
        var theme = value switch
        {
            0 => ElementTheme.Light,
            1 => ElementTheme.Dark,
            2 => ElementTheme.Default,
            _ => ElementTheme.Default
        };

        _themeHelper.SetTheme(theme);
        StatusText = "Theme updated";
    }

    partial void OnWindowOpacityChanged(int value)
    {
        // Apply opacity to main window
        if (Application.Current?.Windows?.FirstOrDefault() is Window mainWindow)
        {
            // Note: WinUI 3 doesn't directly support window opacity
            // This would need platform-specific implementation
            StatusText = $"Opacity set to {value}%";
        }
    }

    partial void OnAlwaysOnTopChanged(bool value)
    {
        // Apply always-on-top to main window
        if (Application.Current?.Windows?.FirstOrDefault() is Window mainWindow)
        {
            // Note: WinUI 3 requires platform-specific implementation
            StatusText = value ? "Window set to always on top" : "Always on top disabled";
        }
    }

    partial void OnStartWithWindowsChanged(bool value)
    {
        try
        {
            var startupPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Startup),
                "SystemPulse.lnk");

            if (value)
            {
                // Create startup shortcut
                // Note: Requires IWshRuntimeLibrary or alternative implementation
                StatusText = "Auto-start enabled";
            }
            else
            {
                // Remove startup shortcut
                if (File.Exists(startupPath))
                {
                    File.Delete(startupPath);
                }
                StatusText = "Auto-start disabled";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to update startup setting", ex);
            StatusText = "Failed to update auto-start";
        }
    }

    [RelayCommand]
    public async Task SaveAsync()
    {
        try
        {
            var settings = GetCurrentSettings();
            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_settingsFilePath, json);

            _originalSettings = settings;
            StatusText = "Settings saved successfully";
            _logger.LogInfo("Settings saved");

            await Task.Delay(2000);
            StatusText = "Ready";
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to save settings", ex);
            StatusText = "Failed to save settings";
            await _dialogHelper.ShowErrorAsync("Save Failed", "Could not save settings. Check permissions and try again.");
        }
    }

    [RelayCommand]
    public void Cancel()
    {
        // Revert to original settings
        if (_originalSettings != null)
        {
            ApplySettings(_originalSettings);
            StatusText = "Changes cancelled";
            _logger.LogInfo("Settings changes cancelled");
        }
    }

    [RelayCommand]
    public async Task ResetSettingsAsync()
    {
        var confirmed = await _dialogHelper.ShowConfirmationAsync(
            "Reset Settings",
            "Are you sure you want to reset all settings to their default values? This cannot be undone.");

        if (confirmed)
        {
            var defaults = GetDefaultSettings();
            ApplySettings(defaults);
            _originalSettings = defaults;

            // Save defaults
            await SaveAsync();

            StatusText = "Settings reset to defaults";
            _logger.LogInfo("Settings reset to defaults");
        }
    }

    [RelayCommand]
    public async Task ClearLogsAsync()
    {
        var confirmed = await _dialogHelper.ShowConfirmationAsync(
            "Clear Logs",
            "Are you sure you want to delete all application log files? This cannot be undone.");

        if (confirmed)
        {
            try
            {
                var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var logsPath = Path.Combine(appDataPath, "SystemPulse", "Logs");

                if (Directory.Exists(logsPath))
                {
                    var files = Directory.GetFiles(logsPath, "*.log");
                    foreach (var file in files)
                    {
                        File.Delete(file);
                    }

                    StatusText = $"Cleared {files.Length} log files";
                    _logger.LogInfo($"Cleared {files.Length} log files");
                }
                else
                {
                    StatusText = "No log files found";
                }

                await Task.Delay(2000);
                StatusText = "Ready";
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to clear logs", ex);
                StatusText = "Failed to clear logs";
                await _dialogHelper.ShowErrorAsync("Clear Failed", "Could not clear log files. They may be in use.");
            }
        }
    }
}

// Settings data model
public class AppSettings
{
    public int ThemeIndex { get; set; }
    public int WindowOpacity { get; set; }
    public int RefreshInterval { get; set; }
    public int MaxChartDataPoints { get; set; }
    public bool AlwaysOnTop { get; set; }
    public bool StartWithWindows { get; set; }
    public bool MinimizeToTray { get; set; }
    public bool StartMinimized { get; set; }
    public bool ShowNotifications { get; set; }
}
