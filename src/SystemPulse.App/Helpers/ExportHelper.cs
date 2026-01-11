using SystemPulse.App.Models;

namespace SystemPulse.App.Helpers;

public static class ExportHelper
{
    /// <summary>
    /// Exports performance metrics to CSV format
    /// </summary>
    public static string ExportMetricsToCSV(
        List<PerformanceMetrics> metrics,
        string filename = "performance_export.csv")
    {
        var csv = new System.Text.StringBuilder();
        csv.AppendLine("Timestamp,CPU%,RAM%,GPU%,Disk%,Network(Mbps),Processes,Threads");

        foreach (var metric in metrics)
        {
            csv.AppendLine($"{metric.Timestamp:yyyy-MM-dd HH:mm:ss}," +
                          $"{metric.CPUUsage:F2}," +
                          $"{metric.RAMUsagePercent:F2}," +
                          $"{metric.GPUUsage:F2}," +
                          $"{metric.DiskUsagePercent:F2}," +
                          $"{metric.NetworkUsageMbps:F2}," +
                          $"{metric.ProcessCount}," +
                          $"{metric.ThreadCount}");
        }

        return csv.ToString();
    }

    /// <summary>
    /// Exports process list to CSV format
    /// </summary>
    public static string ExportProcessesToCSV(List<ProcessInfo> processes)
    {
        var csv = new System.Text.StringBuilder();
        csv.AppendLine("PID,Name,CPU%,Memory(MB),Threads,User,Status,Priority");

        foreach (var process in processes)
        {
            var memoryMB = process.MemoryUsage / 1024 / 1024;
            csv.AppendLine($"{process.PID}," +
                          $"\"{process.Name}\"," +
                          $"{process.CPUUsage:F2}," +
                          $"{memoryMB}," +
                          $"{process.ThreadCount}," +
                          $"{process.User}," +
                          $"{process.State}," +
                          $"{process.Priority}");
        }

        return csv.ToString();
    }

    /// <summary>
    /// Saves content to file
    /// </summary>
    public static async Task<bool> SaveToFileAsync(string content, string filename)
    {
        try
        {
            var downloadsFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var filepath = Path.Combine(downloadsFolder, "Downloads", filename);

            Directory.CreateDirectory(Path.GetDirectoryName(filepath));
            await File.WriteAllTextAsync(filepath, content);

            return true;
        }
        catch
        {
            return false;
        }
    }
}
