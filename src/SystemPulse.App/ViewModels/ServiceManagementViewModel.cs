using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SystemPulse.App.Models;
using SystemPulse.App.Services;
using System.Collections.ObjectModel;

namespace SystemPulse.App.ViewModels;

public partial class ServiceManagementViewModel : ObservableObject
{
    private readonly IWMIService _wmiService;
    private readonly ILoggingService _logger;
    private List<ServiceInfo> _allServices = new();

    [ObservableProperty]
    private ObservableCollection<ServiceInfo> services = new();

    [ObservableProperty]
    private string searchQuery = string.Empty;

    [ObservableProperty]
    private ServiceState? statusFilter = null;

    [ObservableProperty]
    private ServiceInfo selectedService;

    [ObservableProperty]
    private string statusText = "Loading services...";

    [ObservableProperty]
    private bool isLoading = false;

    public ServiceManagementViewModel(IWMIService wmiService, ILoggingService logger)
    {
        _wmiService = wmiService ?? throw new ArgumentNullException(nameof(wmiService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [RelayCommand]
    public void LoadServices()
    {
        try
        {
            IsLoading = true;
            StatusText = "Loading services...";

            _allServices = _wmiService.GetServices();
            ApplyFiltersAndSort();

            StatusText = $"Loaded {Services.Count} services";
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to load services", ex);
            StatusText = "Error loading services";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public void FilterByStatus(ServiceState status)
    {
        StatusFilter = StatusFilter == status ? null : status;
        ApplyFiltersAndSort();
    }

    [RelayCommand]
    public void SearchServices()
    {
        ApplyFiltersAndSort();
    }

    [RelayCommand]
    public async Task StartServiceAsync(ServiceInfo service)
    {
        if (service == null || service.State == ServiceState.Running)
            return;

        try
        {
            IsLoading = true;
            var success = await _wmiService.StartServiceAsync(service.Name);

            if (success)
            {
                service.State = ServiceState.StartPending;
                StatusText = $"Starting {service.DisplayName}...";
                _logger.LogInfo($"Started service: {service.Name}");
                
                // Reload after delay
                await Task.Delay(1000);
                LoadServices();
            }
            else
            {
                StatusText = $"Failed to start {service.DisplayName}";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to start service {service.Name}", ex);
            StatusText = "Error starting service";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public async Task StopServiceAsync(ServiceInfo service)
    {
        if (service == null || service.State == ServiceState.Stopped)
            return;

        try
        {
            IsLoading = true;
            var success = await _wmiService.StopServiceAsync(service.Name);

            if (success)
            {
                service.State = ServiceState.StopPending;
                StatusText = $"Stopping {service.DisplayName}...";
                _logger.LogInfo($"Stopped service: {service.Name}");
                
                // Reload after delay
                await Task.Delay(1000);
                LoadServices();
            }
            else
            {
                StatusText = $"Failed to stop {service.DisplayName}";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to stop service {service.Name}", ex);
            StatusText = "Error stopping service";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public async Task RestartServiceAsync(ServiceInfo service)
    {
        if (service == null)
            return;

        try
        {
            IsLoading = true;
            StatusText = $"Restarting {service.DisplayName}...";
            
            var success = await _wmiService.RestartServiceAsync(service.Name);

            if (success)
            {
                _logger.LogInfo($"Restarted service: {service.Name}");
                
                // Reload after delay
                await Task.Delay(2000);
                LoadServices();
            }
            else
            {
                StatusText = $"Failed to restart {service.DisplayName}";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to restart service {service.Name}", ex);
            StatusText = "Error restarting service";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void ApplyFiltersAndSort()
    {
        var filtered = _allServices.AsEnumerable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(SearchQuery))
        {
            filtered = filtered.Where(s =>
                s.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                s.DisplayName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                s.Description.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase));
        }

        // Apply status filter
        if (StatusFilter.HasValue)
        {
            filtered = filtered.Where(s => s.State == StatusFilter.Value);
        }

        // Sort by display name
        var sorted = filtered.OrderBy(s => s.DisplayName);

        Services.Clear();
        foreach (var service in sorted)
            Services.Add(service);
    }
}
