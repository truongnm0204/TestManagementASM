using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TestManagementASM.Models;
using TestManagementASM.Services;
using TestManagementASM.Services.Interfaces;
using TestManagementASM.Stores;
using TestManagementASM.ViewModels.Base;

namespace TestManagementASM.ViewModels.Student;

public class StudentTestListViewModel : ViewModelBase
{
    private readonly ITestService _testService;
    private readonly AuthStore _authStore;
    private readonly SelectedTestStore _selectedTestStore;
    private readonly INavigationService _navigationService;

    private ObservableCollection<Test> _availableTests = new();
    private Test? _selectedTest;
    private bool _isLoading;

    public ObservableCollection<Test> AvailableTests
    {
        get => _availableTests;
        set => SetProperty(ref _availableTests, value);
    }

    public Test? SelectedTest
    {
        get => _selectedTest;
        set => SetProperty(ref _selectedTest, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public ICommand LoadTestsCommand { get; }
    public ICommand ViewTestDetailCommand { get; }
    public ICommand RefreshCommand { get; }

    public StudentTestListViewModel(
        ITestService testService,
        AuthStore authStore,
        SelectedTestStore selectedTestStore,
        INavigationService navigationService)
    {
        _testService = testService;
        _authStore = authStore;
        _selectedTestStore = selectedTestStore;
        _navigationService = navigationService;

        LoadTestsCommand = new RelayCommand(async () => await LoadTestsAsync());
        RefreshCommand = new RelayCommand(async () => await LoadTestsAsync());
        ViewTestDetailCommand = new RelayCommand(ViewTestDetail, () => SelectedTest != null);

        _ = LoadTestsAsync();
    }

    private async Task LoadTestsAsync()
    {
        try
        {
            IsLoading = true;

            if (_authStore.CurrentUser == null)
            {
                MessageBox.Show("Vui lòng đăng nhập để xem danh sách bài thi.", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var tests = await _testService.GetAvailableTestsForStudentAsync(_authStore.CurrentUser.UserId);
            AvailableTests = new ObservableCollection<Test>(tests);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi tải danh sách bài thi: {ex.Message}", "Lỗi",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void ViewTestDetail()
    {
        if (SelectedTest != null)
        {
            _selectedTestStore.SelectedTest = SelectedTest;
            _navigationService.NavigateTo<TestDetailViewModel>();
        }
    }
}

