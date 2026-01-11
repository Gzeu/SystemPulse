using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SystemPulse.App.Services;

namespace SystemPulse.App.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly ISettingsService _settingsService;
    private readonly ILoggingService _logger;

    [ObservableProperty]
    private string selectedTheme = "System";

    [ObservableProperty]
    private int refreshInterval = 2;

    [ObservableProperty]
    private double windowOpacity = 1.0;

    [ObservableProperty]
    private bool alwaysOnTop = false;

    public SettingsViewModel(ISettingsService settingsService, ILoggingService logger)
    {
        _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        LoadSettings();
    }

    private void LoadSettings()
    {
        try
        {
            SelectedTheme = _settingsService.GetTheme();
            RefreshInterval = _settingsService.GetRefreshInterval();
            WindowOpacity = _settingsService.GetWindowOpacity();
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to load settings", ex);
        }
    }

    [RelayCommand]
    public async Task SaveSettingsAsync()
    {
        try
        {
            await _settingsService.SetThemeAsync(SelectedTheme);
            await _settingsService.SetRefreshIntervalAsync(RefreshInterval);
            await _settingsService.SetWindowOpacityAsync(WindowOpacity);
            _logger.LogInfo("Settings saved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to save settings", ex);
        }
    }
}
