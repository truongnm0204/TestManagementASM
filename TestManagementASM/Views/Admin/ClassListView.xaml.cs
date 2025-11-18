using System.Windows;
using System.Windows.Controls;
using TestManagementASM.ViewModels;

namespace TestManagementASM.Views.Admin;

public partial class ClassListView : UserControl
{
    public ClassListView()
    {
        InitializeComponent();
        Loaded += (s, e) =>
        {
            if (DataContext is ClassListViewModel vm)
            {
                vm.OnShowClassForm += (formVm) =>
                {
                    var view = new ClassFormView { DataContext = formVm };
                    var window = new Window
                    {
                        Content = view,
                        Title = "Thêm/Sửa Lớp Học",
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

