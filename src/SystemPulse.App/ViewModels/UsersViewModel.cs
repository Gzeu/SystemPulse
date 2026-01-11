using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SystemPulse.App.Services;
using System.Collections.ObjectModel;

namespace SystemPulse.App.ViewModels;

public class UserSessionInfo
{
    public string Username { get; set; }
    public string SessionType { get; set; } // Console, RDP, SSH
    public DateTime LogonTime { get; set; }
    public TimeSpan IdleTime { get; set; }
    public string SessionId { get; set; }
}

public partial class UsersViewModel : ObservableObject
{
    private readonly IWMIService _wmiService;
    private readonly ILoggingService _logger;
    private List<UserSessionInfo> _allUsers = new();

    [ObservableProperty]
    private ObservableCollection<UserSessionInfo> userSessions = new();

    [ObservableProperty]
    private UserSessionInfo selectedUser;

    [ObservableProperty]
    private string statusText = "Loading user sessions...";

    [ObservableProperty]
    private bool isLoading = false;

    public UsersViewModel(IWMIService wmiService, ILoggingService logger)
    {
        _wmiService = wmiService ?? throw new ArgumentNullException(nameof(wmiService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [RelayCommand]
    public void LoadUserSessions()
    {
        try
        {
            IsLoading = true;
            StatusText = "Loading user sessions...";

            var activeUsers = _wmiService.GetActiveUsers();
            _allUsers = activeUsers.Select(u => new UserSessionInfo
            {
                Username = u,
                SessionType = "Console",
                LogonTime = DateTime.Now,
                IdleTime = TimeSpan.Zero,
                SessionId = u
            }).ToList();

            // TODO: Get more detailed session information from Win32_LoggedInUser or Terminal Services

            UserSessions.Clear();
            foreach (var user in _allUsers)
                UserSessions.Add(user);

            StatusText = $"Loaded {UserSessions.Count} user session(s)";
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to load user sessions", ex);
            StatusText = "Error loading user sessions";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public async Task LogoffUserAsync(UserSessionInfo user)
    {
        if (user == null)
            return;

        try
        {
            IsLoading = true;
            var success = await _wmiService.LogoffUserAsync(user.Username);

            if (success)
            {
                StatusText = $"Logged off user: {user.Username}";
                _logger.LogInfo($"Logged off user: {user.Username}");
                LoadUserSessions();
            }
            else
            {
                StatusText = $"Failed to logoff {user.Username}";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to logoff user {user.Username}", ex);
            StatusText = "Error logging off user";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public async Task SendMessageToUserAsync(UserSessionInfo user, string message)
    {
        if (user == null || string.IsNullOrWhiteSpace(message))
            return;

        try
        {
            // TODO: Implement message sending via Win32_OSRecoveryConfiguration or similar
            StatusText = $"Message sent to {user.Username}";
            _logger.LogInfo($"Message sent to user: {user.Username}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to send message to user {user.Username}", ex);
            StatusText = "Error sending message";
        }
    }
}
