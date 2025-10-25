using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace TestManagementASM.Views;

public partial class UserListView : UserControl
{
    public UserListView()
    {
        InitializeComponent();
    }
}

public class StatusToTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int status)
        {
            return status switch
            {
                1 => "Hoạt động",
                0 => "Không hoạt động",
                2 => "Bị khóa",
                _ => "Không xác định"
            };
        }
        return "Không xác định";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
