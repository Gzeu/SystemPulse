namespace SystemPulse.App.Services;

public interface ISettingsService
{
    T GetSetting<T>(string key, T defaultValue = default);
    Task SetSettingAsync<T>(string key, T value);
    string GetTheme();
    Task SetThemeAsync(string theme);
    int GetRefreshInterval();
    Task SetRefreshIntervalAsync(int seconds);
    double GetWindowOpacity();
    Task SetWindowOpacityAsync(double opacity);
}
