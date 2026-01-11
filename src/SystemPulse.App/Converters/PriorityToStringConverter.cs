using Microsoft.UI.Xaml.Data;

namespace SystemPulse.App.Converters;

public class PriorityToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is ProcessPriority priority)
        {
            return priority switch
            {
                ProcessPriority.RealTime => "Realtime",
                ProcessPriority.High => "High",
                ProcessPriority.AboveNormal => "Above Normal",
                ProcessPriority.Normal => "Normal",
                ProcessPriority.BelowNormal => "Below Normal",
                ProcessPriority.Low => "Low",
                ProcessPriority.Idle => "Idle",
                _ => "Normal"
            };
        }

        return "Normal";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
