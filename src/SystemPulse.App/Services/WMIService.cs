using System.Management;
using SystemPulse.App.Models;

namespace SystemPulse.App.Services;

public class WMIService : IWMIService
{
    private readonly ILoggingService _logger;

    public WMIService(ILoggingService logger)
    {
        _logger = logger;
    }

    public float GetGPUUsage()
    {
        try
        {
            var searcher = new ManagementObjectSearcher(
                "root\\\\cimv2",
                "SELECT AdapterRAM FROM Win32_VideoController");

            foreach (var obj in searcher.Get())
            {
                return 0; // TODO: Implement full GPU monitoring
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Failed to get GPU usage", ex);
        }

        return 0;
    }

    public float GetGPUMemoryUsage()
    {
        return 0; // TODO: Implement GPU memory monitoring
    }

    public List<ServiceInfo> GetServices()
    {
        var services = new List<ServiceInfo>();

        try
        {
            var searcher = new ManagementObjectSearcher(
                "SELECT Name, DisplayName, State, StartMode, Description FROM Win32_Service");

            foreach (var service in searcher.Get())
            {
                var name = service["Name"]?.ToString() ?? "Unknown";
                var displayName = service["DisplayName"]?.ToString() ?? name;
                var state = service["State"]?.ToString() ?? "Unknown";
                var startMode = service["StartMode"]?.ToString() ?? "Unknown";
                var description = service["Description"]?.ToString() ?? "N/A";

                services.Add(new ServiceInfo
                {
                    Name = name,
                    DisplayName = displayName,
                    Description = description,
                    State = (ServiceState)Enum.Parse(typeof(ServiceState), state, true),
                    StartMode = (ServiceStartMode)Enum.Parse(typeof(ServiceStartMode), startMode, true)
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to get services", ex);
        }

        return services;
    }

    public async Task<bool> StartServiceAsync(string serviceName)
    {
        return await Task.Run(() =>
        {
            try
            {
                var searcher = new ManagementObjectSearcher(
                    $"SELECT * FROM Win32_Service WHERE Name = '{serviceName}'");

                foreach (var service in searcher.Get())
                {
                    service.InvokeMethod("StartService", null);
                    _logger.LogInfo($"Started service: {serviceName}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to start service {serviceName}", ex);
            }

            return false;
        });
    }

    public async Task<bool> StopServiceAsync(string serviceName)
    {
        return await Task.Run(() =>
        {
            try
            {
                var searcher = new ManagementObjectSearcher(
                    $"SELECT * FROM Win32_Service WHERE Name = '{serviceName}'");

                foreach (var service in searcher.Get())
                {
                    service.InvokeMethod("StopService", null);
                    _logger.LogInfo($"Stopped service: {serviceName}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to stop service {serviceName}", ex);
            }

            return false;
        });
    }

    public async Task<bool> RestartServiceAsync(string serviceName)
    {
        var stopped = await StopServiceAsync(serviceName);
        if (stopped)
        {
            await Task.Delay(1000);
            return await StartServiceAsync(serviceName);
        }
        return false;
    }

    public List<string> GetActiveUsers()
    {
        var users = new List<string>();

        try
        {
            var searcher = new ManagementObjectSearcher(
                "SELECT UserName FROM Win32_ComputerSystemProduct");

            foreach (var item in searcher.Get())
            {
                var user = item["UserName"]?.ToString();
                if (!string.IsNullOrEmpty(user))
                    users.Add(user);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Failed to get active users", ex);
        }

        return users;
    }

    public async Task<bool> LogoffUserAsync(string username)
    {
        return await Task.Run(() =>
        {
            try
            {
                _logger.LogInfo($"Logoff requested for user: {username}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to logoff user {username}", ex);
                return false;
            }
        });
    }
}
