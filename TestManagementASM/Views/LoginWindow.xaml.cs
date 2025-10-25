using System.Windows;
using System.Windows.Controls;

namespace TestManagementASM.Views;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
    }

    private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext != null)
        {
            ((dynamic)DataContext).Password = ((PasswordBox)sender).Password;
        }
    }
}
