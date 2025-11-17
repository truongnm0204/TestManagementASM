using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TestManagementASM.Converters;

/// <summary>
/// Converts RoleId to Visibility for menu items
/// Usage: ConverterParameter should be the target RoleId (1=Admin, 2=Teacher, 3=Student)
/// </summary>
public class RoleToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return Visibility.Collapsed;

        if (value is int currentRoleId && parameter is string targetRoleIdString)
        {
            if (int.TryParse(targetRoleIdString, out int targetRoleId))
            {
                return currentRoleId == targetRoleId ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

