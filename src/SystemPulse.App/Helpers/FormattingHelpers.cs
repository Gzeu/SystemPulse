namespace SystemPulse.App.Helpers;

public static class FormattingHelpers
{
    public static string FormatBytes(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        double len = bytes;
        int order = 0;

        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }

        return $"{len:F2} {sizes[order]}";
    }

    public static string FormatUptime(DateTime startTime)
    {
        var uptime = DateTime.Now - startTime;
        return $"{uptime.Days}d {uptime.Hours}h {uptime.Minutes}m";
    }

    public static string FormatPercentage(float value)
    {
        return $"{Math.Min(100, Math.Max(0, value)):F1}%";
    }

    public static string FormatNetworkSpeed(float bytesPerSec)
    {
        return FormatBytes((long)bytesPerSec) + "/s";
    }
}
