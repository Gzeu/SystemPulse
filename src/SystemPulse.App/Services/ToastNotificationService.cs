using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;
using SystemPulse.App.Services;

namespace SystemPulse.App.Services;

public interface IToastNotificationService
{
    void Initialize();
    void ShowInfo(string title, string message);
    void ShowWarning(string title, string message);
    void ShowError(string title, string message);
    void ShowCpuAlert(double cpuUsage);
    void ShowMemoryAlert(double memoryUsage);
    void Dispose();
}

public class ToastNotificationService : IToastNotificationService
{
    private readonly ILoggingService _logger;
    private bool _isInitialized;
    private bool _notificationsEnabled = true;

    public ToastNotificationService(ILoggingService logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void Initialize()
    {
        if (_isInitialized)
            return;

        try
        {
            // Register with Windows notification system
            AppNotificationManager.Default.NotificationInvoked += OnNotificationInvoked;
            AppNotificationManager.Default.Register();

            _isInitialized = true;
            _logger.LogInfo("Toast notification service initialized");
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to initialize toast notifications", ex);
            _isInitialized = false;
        }
    }

    public void ShowInfo(string title, string message)
    {
        if (!_notificationsEnabled || !_isInitialized)
            return;

        try
        {
            var toast = new AppNotificationBuilder()
                .AddText(title)
                .AddText(message)
                .BuildNotification();

            AppNotificationManager.Default.Show(toast);
            _logger.LogInfo($"Toast notification shown: {title}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to show info toast: {title}", ex);
        }
    }

    public void ShowWarning(string title, string message)
    {
        if (!_notificationsEnabled || !_isInitialized)
            return;

        try
        {
            var toast = new AppNotificationBuilder()
                .AddText(title)
                .AddText(message)
                .AddArgument("action", "warning")
                .BuildNotification();

            AppNotificationManager.Default.Show(toast);
            _logger.LogWarning($"Toast warning shown: {title}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to show warning toast: {title}", ex);
        }
    }

    public void ShowError(string title, string message)
    {
        if (!_notificationsEnabled || !_isInitialized)
            return;

        try
        {
            var toast = new AppNotificationBuilder()
                .AddText(title)
                .AddText(message)
                .AddArgument("action", "error")
                .BuildNotification();

            AppNotificationManager.Default.Show(toast);
            _logger.LogError($"Toast error shown: {title}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to show error toast: {title}", ex);
        }
    }

    public void ShowCpuAlert(double cpuUsage)
    {
        if (!_notificationsEnabled || !_isInitialized)
            return;

        try
        {
            var message = $"CPU usage is at {cpuUsage:F1}%. Check Performance page for details.";

            var toast = new AppNotificationBuilder()
                .AddText("High CPU Usage")
                .AddText(message)
                .AddArgument("action", "cpu_alert")
                .AddArgument("page", "performance")
                .BuildNotification();

            AppNotificationManager.Default.Show(toast);
            _logger.LogWarning($"CPU alert shown: {cpuUsage:F1}%");
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to show CPU alert toast", ex);
        }
    }

    public void ShowMemoryAlert(double memoryUsage)
    {
        if (!_notificationsEnabled || !_isInitialized)
            return;

        try
        {
            var message = $"Memory usage is at {memoryUsage:F1}%. Consider closing some applications.";

            var toast = new AppNotificationBuilder()
                .AddText("High Memory Usage")
                .AddText(message)
                .AddArgument("action", "memory_alert")
                .AddArgument("page", "processes")
                .BuildNotification();

            AppNotificationManager.Default.Show(toast);
            _logger.LogWarning($"Memory alert shown: {memoryUsage:F1}%");
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to show memory alert toast", ex);
        }
    }

    private void OnNotificationInvoked(AppNotificationManager sender, AppNotificationActivatedEventArgs args)
    {
        // Handle notification clicks
        var arguments = args.Arguments;

        if (arguments.TryGetValue("action", out var action))
        {
            _logger.LogInfo($"Toast notification clicked: {action}");

            // Navigate to appropriate page based on action
            if (arguments.TryGetValue("page", out var page))
            {
                // TODO: Implement navigation from toast click
                _logger.LogInfo($"Navigate to page: {page}");
            }
        }
    }

    public void SetNotificationsEnabled(bool enabled)
    {
        _notificationsEnabled = enabled;
        _logger.LogInfo($"Notifications {(enabled ? "enabled" : "disabled")}");
    }

    public void Dispose()
    {
        if (_isInitialized)
        {
            try
            {
                AppNotificationManager.Default.Unregister();
                _logger.LogInfo("Toast notification service disposed");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error disposing toast notification service", ex);
            }

            _isInitialized = false;
        }
    }
}
