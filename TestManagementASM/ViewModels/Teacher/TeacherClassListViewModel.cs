using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TestManagementASM.Commands;
using TestManagementASM.Models;
using TestManagementASM.Services.Interfaces;
using TestManagementASM.Stores;
using TestManagementASM.ViewModels.Base;

namespace TestManagementASM.ViewModels.Teacher;

public class TeacherClassListViewModel : ViewModelBase
{
    private readonly IClassService _classService;
    private readonly AuthStore _authStore;
    private readonly SelectedClassStore _selectedClassStore;
    private readonly INavigationService _navigationService;

    private ObservableCollection<Class> _managedClasses = new();
    private Class? _selectedClass;
    private bool _isLoading;

    public ObservableCollection<Class> ManagedClasses
    {
        get => _managedClasses;
        set => SetProperty(ref _managedClasses, value);
    }

    public Class? SelectedClass
    {
        get => _selectedClass;
        set => SetProperty(ref _selectedClass, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public ICommand LoadClassesCommand { get; }
    public ICommand ViewClassDetailCommand { get; }
    public ICommand RefreshCommand { get; }

    public TeacherClassListViewModel(
        IClassService classService,
        AuthStore authStore,
        SelectedClassStore selectedClassStore,
        INavigationService navigationService)
    {
        _classService = classService;
        _authStore = authStore;
        _selectedClassStore = selectedClassStore;
        _navigationService = navigationService;

        LoadClassesCommand = new RelayCommand(async () => await LoadClassesAsync());
        RefreshCommand = new RelayCommand(async () => await LoadClassesAsync());
        ViewClassDetailCommand = new RelayCommand(ViewClassDetail, () => SelectedClass != null);

        _ = LoadClassesAsync();
    }

    private async Task LoadClassesAsync()
    {
        try
        {
            IsLoading = true;

            if (_authStore.CurrentUser == null)
            {
                MessageBox.Show("Vui lòng đăng nhập để xem danh sách lớp học.", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var classes = await _classService.GetTeacherClassesAsync(_authStore.CurrentUser.UserId);
            ManagedClasses = new ObservableCollection<Class>(classes);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi tải danh sách lớp học: {ex.Message}", "Lỗi",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void ViewClassDetail()
    {
        if (SelectedClass != null)
        {
            _selectedClassStore.SelectedClass = SelectedClass;
            _navigationService.NavigateTo<ClassDetailViewModel>();
        }
    }
}

