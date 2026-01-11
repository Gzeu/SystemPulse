namespace SystemPulse.App.Models;

public class ServiceInfo
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public ServiceState State { get; set; }
    public ServiceStartMode StartMode { get; set; }
    public string ProcessName { get; set; }
}

public enum ServiceState
{
    Running,
    Stopped,
    StartPending,
    StopPending
}

public enum ServiceStartMode
{
    Boot,
    System,
    Automatic,
    Manual,
    Disabled
}
