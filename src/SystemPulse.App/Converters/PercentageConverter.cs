using Microsoft.UI.Xaml.Data;
using SystemPulse.App.Helpers;

namespace SystemPulse.App.Converters;

public class PercentageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is float percentage)
        {
            return FormattingHelpers.FormatPercentage(percentage);
        }

        if (value is double doubleValue)
        {
            return FormattingHelpers.FormatPercentage((float)doubleValue);
        }

        return "0.0%";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
