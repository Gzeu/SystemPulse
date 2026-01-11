using Microsoft.UI.Xaml.Data;
using SystemPulse.App.Models;

namespace SystemPulse.App.Converters;

public class StatusTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is ProcessState state)
        {
            return state switch
            {
                ProcessState.Running => "Running",
                ProcessState.Suspended => "Suspended",
                ProcessState.Terminated => "Terminated",
                _ => "Unknown"
            };
        }

        return "Unknown";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
