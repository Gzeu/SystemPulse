using System.Diagnostics;

namespace SystemPulse.App.Models;

public class ProcessInfo
{
    public int PID { get; set; }
    public string Name { get; set; }
    public string FullPath { get; set; }
    public float CPUUsage { get; set; }
    public long MemoryUsage { get; set; }
    public long DiskIOBytesPerSec { get; set; }
    public float GPUUsage { get; set; }
    public string User { get; set; }
    public string CommandLine { get; set; }
    public DateTime StartTime { get; set; }
    public ProcessState State { get; set; }
    public ProcessPriority Priority { get; set; }
    public int ThreadCount { get; set; }
    public byte[] Icon { get; set; }
    public bool IsSystemProcess { get; set; }
}

public enum ProcessState
{
    Running = 0,
    Suspended = 1,
    Terminated = 2
}
