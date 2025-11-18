using System.Windows;
using System.Windows.Controls;
using TestManagementASM.ViewModels;
using TestManagementASM.Helpers;

namespace TestManagementASM.Views.Admin;

public partial class UserFormView : UserControl
{
    public UserFormView()
    {
        InitializeComponent();
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is UserFormViewModel vm && !vm.IsEditMode)
        {
            // Get password from PasswordBox and hash it
            var password = PasswordBox.Password;
            if (!string.IsNullOrEmpty(password))
            {
                vm.User.PasswordHash = PasswordHasher.HashPassword(password);
            }
        }
    }
}

