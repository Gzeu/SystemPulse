using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using SystemPulse.App.Models;

namespace SystemPulse.App.Converters;

public class ProcessStateColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is ProcessState state)
        {
            var color = state switch
            {
                ProcessState.Running => new SolidColorBrush(Windows.UI.Color.FromArgb(255, 16, 185, 129)),      // Green
                ProcessState.Suspended => new SolidColorBrush(Windows.UI.Color.FromArgb(255, 245, 158, 11)),    // Orange
                ProcessState.Terminated => new SolidColorBrush(Windows.UI.Color.FromArgb(255, 239, 68, 68)),    // Red
                _ => new SolidColorBrush(Windows.UI.Color.FromArgb(255, 107, 114, 128))                          // Gray
            };

            return color;
        }

        return new SolidColorBrush(Windows.UI.Color.FromArgb(255, 107, 114, 128));
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
