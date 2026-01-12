using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SystemPulse.App.Helpers;
using SystemPulse.App.Models;
using SystemPulse.App.Services;
using Microsoft.UI.Xaml;
using System.Text.Json;

namespace SystemPulse.App.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly ILoggingService _logger;
    private readonly string _settingsFilePath;
    private AppSettings _originalSettings;
    private Window? _mainWindow;

    #region Appearance Settings

    [ObservableProperty]
    private int selectedThemeIndex;

    [ObservableProperty]
    private double windowOpacity = 1.0;

    #endregion

    #region Performance Settings

    [ObservableProperty]
    private int refreshInterval = 2;

    [ObservableProperty]
    private int chartHistory = 300;

    #endregion

    #region Behavior Settings

    [ObservableProperty]
    private bool alwaysOnTop;

    [ObservableProperty]
    private bool startWithWindows;

    [ObservableProperty]
    private bool minimizeToTray;

    [ObservableProperty]
    private bool startMinimized;

    [ObservableProperty]
    private bool showNotifications = true;

    #endregion

    #region Status

    [ObservableProperty]
    private string statusText = string.Empty;

    [ObservableProperty]
    private bool hasUnsavedChanges;

    #endregion

    public SettingsViewModel(ILoggingService logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        // Get settings file path
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appFolder = Path.Combine(appDataPath, "SystemPulse");
        Directory.CreateDirectory(appFolder);
        _settingsFilePath = Path.Combine(appFolder, "settings.json");

        // Load settings
        LoadSettings();

        // Track original settings for cancel
        _originalSettings = GetCurrentSettings();

        // Watch for property changes
        PropertyChanged += (s, e) =>
        {
            if (e.PropertyName != nameof(StatusText) && e.PropertyName != nameof(HasUnsavedChanges))
            {
                HasUnsavedChanges = true;
            }
        };
    }

    public void SetMainWindow(Window window)
    {
        _mainWindow = window;
        ApplyWindowSettings();
    }

    partial void OnWindowOpacityChanged(double value)
    {
        ApplyOpacity();
    }

    partial void OnAlwaysOnTopChanged(bool value)
    {
        ApplyAlwaysOnTop();
    }

    partial void OnSelectedThemeIndexChanged(int value)
    {
        ApplyTheme();
    }

    private void ApplyWindowSettings()
    {
        if (_mainWindow == null)
            return;

        ApplyOpacity();
        ApplyAlwaysOnTop();
    }

    private void ApplyOpacity()
    {
        if (_mainWindow == null)
            return;

        try
        {
            bool success = Win32Helper.SetWindowOpacity(_mainWindow, WindowOpacity);
            if (!success)
            {
                _logger.LogWarning($"Failed to set window opacity to {WindowOpacity}");
            }
            else
            {
                _logger.LogInfo($"Window opacity set to {WindowOpacity * 100}%");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to apply window opacity", ex);
        }
    }

    private void ApplyAlwaysOnTop()
    {
        if (_mainWindow == null)
            return;

        try
        {
            bool success = Win32Helper.SetAlwaysOnTop(_mainWindow, AlwaysOnTop);
            if (!success)
            {
                _logger.LogWarning($"Failed to set always-on-top to {AlwaysOnTop}");
            }
            else
            {
                _logger.LogInfo($"Always-on-top set to {AlwaysOnTop}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to apply always-on-top", ex);
        }
    }

    private void ApplyTheme()
    {
        try
        {
            var theme = SelectedThemeIndex switch
            {
                0 => ElementTheme.Light,
                1 => ElementTheme.Dark,
                2 => ElementTheme.Default,
                _ => ElementTheme.Default
            };

            ThemeHelper.SetAppTheme(theme);
            _logger.LogInfo($"Theme changed to {theme}");
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to apply theme", ex);
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        try
        {
            var settings = GetCurrentSettings();
            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_settingsFilePath, json);

            _originalSettings = settings;
            HasUnsavedChanges = false;
            StatusText = "Settings saved successfully";
            _logger.LogInfo("Settings saved");

            // Clear status after 3 seconds
            await Task.Delay(3000);
            StatusText = string.Empty;
        }
        catch (Exception ex)
        {
            StatusText = "Failed to save settings";
            _logger.LogError("Failed to save settings", ex);
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        try
        {
            ApplySettings(_originalSettings);
            HasUnsavedChanges = false;
            StatusText = "Changes cancelled";
            _logger.LogInfo("Settings cancelled");
        }
        catch (Exception ex)
        {
            StatusText = "Failed to cancel changes";
            _logger.LogError("Failed to cancel settings", ex);
        }
    }

    [RelayCommand]
    private async Task ClearLogsAsync()
    {
        try
        {
            var logsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SystemPulse", "Logs");
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
                StatusText = "No logs to clear";
            }

            await Task.Delay(3000);
            StatusText = string.Empty;
        }
        catch (Exception ex)
        {
            StatusText = "Failed to clear logs";
            _logger.LogError("Failed to clear logs", ex);
        }
    }

    [RelayCommand]
    private async Task ResetToDefaultsAsync()
    {
        try
        {
            var defaults = new AppSettings();
            ApplySettings(defaults);
            await SaveAsync();
            StatusText = "Reset to defaults";
            _logger.LogInfo("Settings reset to defaults");

            await Task.Delay(3000);
            StatusText = string.Empty;
        }
        catch (Exception ex)
        {
            StatusText = "Failed to reset settings";
            _logger.LogError("Failed to reset settings", ex);
        }
    }

    private void LoadSettings()
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
                    _logger.LogInfo("Settings loaded from file");
                    return;
                }
            }

            // Use defaults if file doesn't exist
            _logger.LogInfo("Using default settings");
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to load settings, using defaults", ex);
        }
    }

    private AppSettings GetCurrentSettings()
    {
        return new AppSettings
        {
            ThemeIndex = SelectedThemeIndex,
            WindowOpacity = WindowOpacity,
            RefreshInterval = RefreshInterval,
            ChartHistory = ChartHistory,
            AlwaysOnTop = AlwaysOnTop,
            StartWithWindows = StartWithWindows,
            MinimizeToTray = MinimizeToTray,
            StartMinimized = StartMinimized,
            ShowNotifications = ShowNotifications
        };
    }

    private void ApplySettings(AppSettings settings)
    {
        SelectedThemeIndex = settings.ThemeIndex;
        WindowOpacity = settings.WindowOpacity;
        RefreshInterval = settings.RefreshInterval;
        ChartHistory = settings.ChartHistory;
        AlwaysOnTop = settings.AlwaysOnTop;
        StartWithWindows = settings.StartWithWindows;
        MinimizeToTray = settings.MinimizeToTray;
        StartMinimized = settings.StartMinimized;
        ShowNotifications = settings.ShowNotifications;
    }

    public AppSettings GetSettings() => GetCurrentSettings();
}
