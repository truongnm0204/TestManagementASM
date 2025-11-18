using System.Windows;
using System.Windows.Controls;
using TestManagementASM.ViewModels;
using TestManagementASM.Views.Admin;

namespace TestManagementASM.Views;

public partial class SubjectListView : UserControl
{
    public SubjectListView()
    {
        InitializeComponent();
        Loaded += (s, e) =>
        {
            if (DataContext is SubjectListViewModel vm)
            {
                vm.OnShowSubjectForm += (formVm) =>
                {
                    var view = new SubjectFormView { DataContext = formVm };
                    var window = new Window
                    {
                        Content = view,
                        Title = "Thêm/Sửa Môn Học",
                        Width = 500,
                        Height = 300,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner,
                        Owner = Window.GetWindow(this)
                    };
                    window.ShowDialog();
                };
            }
        };
    }
}
