using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TestManagementASM.ViewModels;
using TestManagementASM.Views.Admin;

namespace TestManagementASM.Views;

public partial class UserListView : UserControl
{
    public UserListView()
    {
        InitializeComponent();
        Loaded += (s, e) =>
        {
            if (DataContext is UserListViewModel vm)
            {
                vm.OnShowUserForm += (formVm) =>
                {
                    var view = new UserFormView { DataContext = formVm };
                    var window = new Window
                    {
                        Content = view,
                        Title = "Thêm/Sửa Người Dùng",
                        Width = 500,
                        Height = 400,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner,
                        Owner = Window.GetWindow(this)
                    };
                    window.ShowDialog();
                };
            }
        };
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
