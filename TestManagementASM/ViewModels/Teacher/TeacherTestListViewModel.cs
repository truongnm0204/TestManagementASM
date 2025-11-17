using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TestManagementASM.Commands;
using TestManagementASM.Models;
using TestManagementASM.Services.Interfaces;
using TestManagementASM.Stores;
using TestManagementASM.ViewModels.Base;

namespace TestManagementASM.ViewModels.Teacher;

public class TeacherTestListViewModel : ViewModelBase
{
    private readonly ITestService _testService;
    private readonly AuthStore _authStore;
    private readonly SelectedTestStore _selectedTestStore;
    private readonly INavigationService _navigationService;

    private ObservableCollection<Test> _teacherTests = new();
    private Test? _selectedTest;
    private bool _isLoading;

    public ObservableCollection<Test> TeacherTests
    {
        get => _teacherTests;
        set => SetProperty(ref _teacherTests, value);
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
    public ICommand CreateTestCommand { get; }
    public ICommand EditTestCommand { get; }
    public ICommand DeleteTestCommand { get; }
    public ICommand ToggleActiveCommand { get; }
    public ICommand RefreshCommand { get; }

    public TeacherTestListViewModel(
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
        CreateTestCommand = new RelayCommand(CreateTest);
        EditTestCommand = new RelayCommand(EditTest, () => SelectedTest != null);
        DeleteTestCommand = new RelayCommand(async () => await DeleteTestAsync(), () => SelectedTest != null);
        ToggleActiveCommand = new RelayCommand(async () => await ToggleActiveAsync(), () => SelectedTest != null);

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

            var tests = await _testService.GetTestsByTeacherAsync(_authStore.CurrentUser.UserId);
            TeacherTests = new ObservableCollection<Test>(tests);
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

    private void CreateTest()
    {
        _selectedTestStore.SelectedTest = null;
        _navigationService.NavigateTo<TestFormViewModel>();
    }

    private void EditTest()
    {
        if (SelectedTest != null)
        {
            _selectedTestStore.SelectedTest = SelectedTest;
            _navigationService.NavigateTo<TestFormViewModel>();
        }
    }

    private async Task DeleteTestAsync()
    {
        if (SelectedTest == null) return;

        var result = MessageBox.Show(
            $"Bạn có chắc chắn muốn xóa bài thi '{SelectedTest.TestName}'?\nThao tác này không thể hoàn tác!",
            "Xác nhận xóa",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            try
            {
                IsLoading = true;
                var success = await _testService.DeleteTestAsync(SelectedTest.TestId);

                if (success)
                {
                    MessageBox.Show("Xóa bài thi thành công!", "Thành công",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    await LoadTestsAsync();
                }
                else
                {
                    MessageBox.Show("Không thể xóa bài thi. Vui lòng thử lại!", "Lỗi",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa bài thi: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }

    private async Task ToggleActiveAsync()
    {
        if (SelectedTest == null) return;

        try
        {
            IsLoading = true;
            var success = await _testService.ToggleTestActiveStatusAsync(SelectedTest.TestId);

            if (success)
            {
                var status = SelectedTest.IsActive ? "kích hoạt" : "vô hiệu hóa";
                MessageBox.Show($"Đã {status} bài thi thành công!", "Thành công",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                await LoadTestsAsync();
            }
            else
            {
                MessageBox.Show("Không thể thay đổi trạng thái bài thi. Vui lòng thử lại!", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi thay đổi trạng thái: {ex.Message}", "Lỗi",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }
}

