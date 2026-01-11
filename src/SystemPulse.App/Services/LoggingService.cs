using Serilog;

namespace SystemPulse.App.Services;

public class LoggingService : ILoggingService
{
    private readonly string _logDirectory;

    public LoggingService()
    {
        _logDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "SystemPulse", "logs");

        Directory.CreateDirectory(_logDirectory);
    }

    public void LogDebug(string message)
    {
        Log.Debug(message);
    }

    public void LogInfo(string message)
    {
        Log.Information(message);
    }

    public void LogWarning(string message, Exception ex = null)
    {
        if (ex != null)
            Log.Warning(ex, message);
        else
            Log.Warning(message);
    }

    public void LogError(string message, Exception ex = null)
    {
        if (ex != null)
            Log.Error(ex, message);
        else
            Log.Error(message);
    }

    public async Task<string> ExportSnapshotAsync()
    {
        var snapshotFile = Path.Combine(
            _logDirectory,
            $"snapshot-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.txt");

        var content = new System.Text.StringBuilder();
        content.AppendLine($"SystemPulse Snapshot - {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        content.AppendLine(new string('=', 80));
        content.AppendLine();

        var osVersion = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
        content.AppendLine($"OS: {osVersion}");
        content.AppendLine($"Processors: {Environment.ProcessorCount}");
        content.AppendLine($"Memory: {GC.GetTotalMemory(false) / 1024 / 1024} MB");
        content.AppendLine();

        content.AppendLine("Top Processes by Memory:");
        var processes = System.Diagnostics.Process.GetProcesses()
            .OrderByDescending(p => p.WorkingSet64)
            .Take(10);

        foreach (var proc in processes)
        {
            try
            {
                content.AppendLine($"  {proc.ProcessName,-20} PID: {proc.Id,-6} Memory: {proc.WorkingSet64 / 1024 / 1024} MB");
            }
            catch { }
        }

        await File.WriteAllTextAsync(snapshotFile, content.ToString());
        return snapshotFile;
    }
}
