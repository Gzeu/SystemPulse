namespace SystemPulse.App.Services;

public interface ILoggingService
{
    void LogDebug(string message);
    void LogInfo(string message);
    void LogWarning(string message, Exception ex = null);
    void LogError(string message, Exception ex = null);
    Task<string> ExportSnapshotAsync();
}
