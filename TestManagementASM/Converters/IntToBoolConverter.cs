using System.Globalization;
using System.Windows.Data;

namespace TestManagementASM.Converters;

/// <summary>
/// Converts int to bool (1 = true, 0 = false)
/// </summary>
public class IntToBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int intValue)
        {
            return intValue != 0;
        }
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? 1 : 0;
        }
        return 0;
    }
}

