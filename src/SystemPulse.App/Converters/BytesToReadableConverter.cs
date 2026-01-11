using Microsoft.UI.Xaml.Data;
using SystemPulse.App.Helpers;

namespace SystemPulse.App.Converters;

public class BytesToReadableConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is long bytes)
        {
            return FormattingHelpers.FormatBytes(bytes);
        }

        return "0 B";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
