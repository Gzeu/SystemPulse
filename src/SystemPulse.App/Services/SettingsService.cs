using System.Text.Json;

namespace SystemPulse.App.Services;

public class SettingsService : ISettingsService
{
    private readonly string _settingsFile;
    private Dictionary<string, object> _settings;

    public SettingsService()
    {
        var appDataDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "SystemPulse");

        Directory.CreateDirectory(appDataDir);
        _settingsFile = Path.Combine(appDataDir, "settings.json");

        LoadSettings();
    }

    public T GetSetting<T>(string key, T defaultValue = default)
    {
        if (_settings.TryGetValue(key, out var value))
        {
            if (value is JsonElement element)
            {
                return JsonSerializer.Deserialize<T>(element.GetRawText());
            }
            return (T)value;
        }
        return defaultValue;
    }

    public async Task SetSettingAsync<T>(string key, T value)
    {
        _settings[key] = value;
        await SaveSettingsAsync();
    }

    public string GetTheme()
    {
        return GetSetting("Theme", "System");
    }

    public async Task SetThemeAsync(string theme)
    {
        await SetSettingAsync("Theme", theme);
    }

    public int GetRefreshInterval()
    {
        return GetSetting("RefreshInterval", 2);
    }

    public async Task SetRefreshIntervalAsync(int seconds)
    {
        await SetSettingAsync("RefreshInterval", seconds);
    }

    public double GetWindowOpacity()
    {
        return GetSetting("WindowOpacity", 1.0);
    }

    public async Task SetWindowOpacityAsync(double opacity)
    {
        await SetSettingAsync("WindowOpacity", opacity);
    }

    private void LoadSettings()
    {
        if (File.Exists(_settingsFile))
        {
            try
            {
                var json = File.ReadAllText(_settingsFile);
                _settings = JsonSerializer.Deserialize<Dictionary<string, object>>(json) ?? new();
            }
            catch
            {
                _settings = new();
            }
        }
        else
        {
            _settings = new();
        }
    }

    private async Task SaveSettingsAsync()
    {
        var json = JsonSerializer.Serialize(_settings, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(_settingsFile, json);
    }
}
