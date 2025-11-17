using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TestManagementASM.Converters;

/// <summary>
/// Converts RoleId to Visibility for menu items that should be visible for multiple roles
/// Usage: ConverterParameter should be comma-separated RoleIds (e.g., "1,2" for Admin and Teacher)
/// </summary>
public class MultiRoleToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return Visibility.Collapsed;

        if (value is int currentRoleId && parameter is string targetRoleIdsString)
        {
            var targetRoleIds = targetRoleIdsString
                .Split(',')
                .Select(s => s.Trim())
                .Where(s => int.TryParse(s, out _))
                .Select(int.Parse)
                .ToList();

            return targetRoleIds.Contains(currentRoleId) ? Visibility.Visible : Visibility.Collapsed;
        }

        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

