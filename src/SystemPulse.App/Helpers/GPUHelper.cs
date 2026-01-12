using SharpDX.DXGI;
using SystemPulse.App.Models;
using SystemPulse.App.Services;

namespace SystemPulse.App.Helpers;

public static class GPUHelper
{
    public static float GetGpuUsage()
    {
        try
        {
            using var factory = new Factory1();
            using var adapter = factory.GetAdapter1(0);
            var desc = adapter.Description1;
            
            // This is a simplified metric for GPU "memory load" as a proxy for utilization
            // Real DX12 utilization requires more complex hardware query hooks
            var videoMemoryInfo = adapter.QueryVideoMemoryInfo(0, MemorySegmentGroup.Local);
            if (videoMemoryInfo.Budget == 0) return 0;
            
            return (float)videoMemoryInfo.CurrentUsage / videoMemoryInfo.Budget * 100f;
        }
        catch
        {
            return 0;
        }
    }

    public static string GetGpuName()
    {
        try
        {
            using var factory = new Factory1();
            using var adapter = factory.GetAdapter1(0);
            return adapter.Description1.Description;
        }
        catch
        {
            return "Generic GPU";
        }
    }
}
