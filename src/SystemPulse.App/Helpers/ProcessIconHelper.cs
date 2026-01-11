using System.Drawing;

namespace SystemPulse.App.Helpers;

public static class ProcessIconHelper
{
    public static byte[] ExtractIcon(string processPath)
    {
        try
        {
            if (string.IsNullOrEmpty(processPath) || !File.Exists(processPath))
                return null;

            var icon = Icon.ExtractAssociatedIcon(processPath);
            if (icon == null)
                return null;

            using var ms = new MemoryStream();
            icon.Save(ms);
            return ms.ToArray();
        }
        catch
        {
            return null;
        }
    }
}
