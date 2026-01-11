namespace SystemPulse.App.Models;

public class PerformanceMetrics
{
    public float CPUUsage { get; set; }
    public float RAMUsagePercent { get; set; }
    public long RAMUsageBytes { get; set; }
    public long TotalRAMBytes { get; set; }
    public float GPUUsage { get; set; }
    public float DiskUsagePercent { get; set; }
    public float NetworkUsageMbps { get; set; }
    public int ProcessCount { get; set; }
    public int ThreadCount { get; set; }
    public DateTime Timestamp { get; set; }
}
