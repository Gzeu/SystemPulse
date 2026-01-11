using SystemPulse.App.Models;
using SystemPulse.App.Services;
using System.Text;

namespace SystemPulse.App.Helpers;

public class ExportHelper
{
    private readonly ILoggingService _logger;

    public ExportHelper(ILoggingService logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Exports process list to CSV file
    /// </summary>
    public async Task<bool> ExportProcessListAsync(IEnumerable<ProcessInfo> processes, string filePath)
    {
        try
        {
            var csv = new StringBuilder();
            
            // Header
            csv.AppendLine("Name,PID,CPU %,Memory (MB),Threads,User,Status,Priority,Path");

            // Data rows
            foreach (var process in processes)
            {
                csv.AppendLine($"\"{process.Name}\",{process.PID},{process.CPUUsage:F2},{process.MemoryUsage / 1024 / 1024:F2},{process.ThreadCount},\"{process.Username}\",{process.Status},{process.Priority},\"{process.Path}\"");
            }

            await File.WriteAllTextAsync(filePath, csv.ToString());
            _logger.LogInfo($"Exported {processes.Count()} processes to {filePath}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to export process list to {filePath}", ex);
            return false;
        }
    }

    /// <summary>
    /// Exports performance metrics to CSV file
    /// </summary>
    public async Task<bool> ExportMetricsAsync(IEnumerable<PerformanceMetrics> metrics, string filePath)
    {
        try
        {
            var csv = new StringBuilder();
            
            // Header
            csv.AppendLine("Timestamp,CPU %,RAM %,RAM Used (MB),RAM Total (MB),GPU %,Network (Mbps),Disk %,Processes,Threads");

            // Data rows
            foreach (var metric in metrics)
            {
                csv.AppendLine($"{metric.Timestamp:yyyy-MM-dd HH:mm:ss},{metric.CPUUsage:F2},{metric.RAMUsagePercent:F2},{metric.RAMUsageBytes / 1024 / 1024:F2},{metric.TotalRAMBytes / 1024 / 1024:F2},{metric.GPUUsage:F2},{metric.NetworkUsageMbps:F2},{metric.DiskUsagePercent:F2},{metric.ProcessCount},{metric.ThreadCount}");
            }

            await File.WriteAllTextAsync(filePath, csv.ToString());
            _logger.LogInfo($"Exported {metrics.Count()} metrics to {filePath}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to export metrics to {filePath}", ex);
            return false;
        }
    }

    /// <summary>
    /// Exports performance data to CSV file (for charts)
    /// </summary>
    public async Task<bool> ExportPerformanceDataAsync(Dictionary<string, List<float>> data, string filePath = null)
    {
        try
        {
            // Default file path
            if (string.IsNullOrWhiteSpace(filePath))
            {
                var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                filePath = Path.Combine(desktopPath, $"SystemPulse_Performance_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
            }

            var csv = new StringBuilder();

            // Determine max row count
            int maxRows = data.Values.Max(list => list.Count);

            // Header
            csv.AppendLine(string.Join(",", data.Keys));

            // Data rows
            for (int i = 0; i < maxRows; i++)
            {
                var row = new List<string>();
                foreach (var key in data.Keys)
                {
                    if (i < data[key].Count)
                    {
                        row.Add(data[key][i].ToString("F2"));
                    }
                    else
                    {
                        row.Add(string.Empty);
                    }
                }
                csv.AppendLine(string.Join(",", row));
            }

            await File.WriteAllTextAsync(filePath, csv.ToString());
            _logger.LogInfo($"Exported performance data to {filePath}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to export performance data", ex);
            return false;
        }
    }

    /// <summary>
    /// Exports system snapshot (all current metrics) to text file
    /// </summary>
    public async Task<bool> ExportSystemSnapshotAsync(PerformanceMetrics metrics, IEnumerable<ProcessInfo> processes, string filePath = null)
    {
        try
        {
            // Default file path
            if (string.IsNullOrWhiteSpace(filePath))
            {
                var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                filePath = Path.Combine(desktopPath, $"SystemPulse_Snapshot_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
            }

            var sb = new StringBuilder();
            sb.AppendLine("==================================================");
            sb.AppendLine("SystemPulse - System Snapshot");
            sb.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine("==================================================");
            sb.AppendLine();

            // System Metrics
            sb.AppendLine("SYSTEM METRICS");
            sb.AppendLine("--------------------------------------------------");
            sb.AppendLine($"CPU Usage:        {metrics.CPUUsage:F2}%");
            sb.AppendLine($"RAM Usage:        {metrics.RAMUsagePercent:F2}% ({metrics.RAMUsageBytes / 1024 / 1024:F2} MB / {metrics.TotalRAMBytes / 1024 / 1024:F2} MB)");
            sb.AppendLine($"GPU Usage:        {metrics.GPUUsage:F2}%");
            sb.AppendLine($"Network Activity: {metrics.NetworkUsageMbps:F2} Mbps");
            sb.AppendLine($"Disk Activity:    {metrics.DiskUsagePercent:F2}%");
            sb.AppendLine($"Process Count:    {metrics.ProcessCount}");
            sb.AppendLine($"Thread Count:     {metrics.ThreadCount}");
            sb.AppendLine();

            // Top Processes
            sb.AppendLine("TOP PROCESSES (by CPU)");
            sb.AppendLine("--------------------------------------------------");
            var topProcesses = processes.OrderByDescending(p => p.CPUUsage).Take(10);
            foreach (var process in topProcesses)
            {
                sb.AppendLine($"{process.Name,-30} PID: {process.PID,-8} CPU: {process.CPUUsage:F2}% Memory: {process.MemoryUsage / 1024 / 1024:F2} MB");
            }

            sb.AppendLine();
            sb.AppendLine("==================================================");

            await File.WriteAllTextAsync(filePath, sb.ToString());
            _logger.LogInfo($"Exported system snapshot to {filePath}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to export system snapshot", ex);
            return false;
        }
    }
}
