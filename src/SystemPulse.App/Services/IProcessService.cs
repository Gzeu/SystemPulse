using SystemPulse.App.Models;

namespace SystemPulse.App.Services;

public interface IProcessService
{
    List<ProcessInfo> GetProcesses();
    ProcessInfo GetProcessById(int pid);
    List<ProcessInfo> SearchProcesses(string searchTerm);
    Task KillProcessAsync(int pid);
    Task KillProcessTreeAsync(int pid);
    Task SuspendProcessAsync(int pid);
    Task ResumeProcessAsync(int pid);
    byte[] GetProcessIcon(int pid);
}
