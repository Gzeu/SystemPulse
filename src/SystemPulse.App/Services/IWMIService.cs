using SystemPulse.App.Models;

namespace SystemPulse.App.Services;

public interface IWMIService
{
    float GetGPUUsage();
    float GetGPUMemoryUsage();
    List<ServiceInfo> GetServices();
    Task<bool> StartServiceAsync(string serviceName);
    Task<bool> StopServiceAsync(string serviceName);
    Task<bool> RestartServiceAsync(string serviceName);
    List<string> GetActiveUsers();
    Task<bool> LogoffUserAsync(string username);
}
