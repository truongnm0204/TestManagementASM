using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TestManagementASM.Models;
using TestManagementASM.Services.Interfaces;
using TestManagementASM.Stores;
using TestManagementASM.ViewModels.Base;

namespace TestManagementASM.ViewModels.Student;

public class TestDetailViewModel : ViewModelBase
{
    private readonly ITestService _testService;
    private readonly ITestAttemptService _testAttemptService;
    private readonly AuthStore _authStore;
    private readonly SelectedTestStore _selectedTestStore;
    private readonly INavigationService _navigationService;

    private Test? _currentTest;
    private int _questionCount;
    private bool _canStartTest;
    private ObservableCollection<TestAttempt> _previousAttempts = new();
    private bool _isLoading;

    public Test? CurrentTest
    {
        get => _currentTest;
        set => SetProperty(ref _currentTest, value);
    }

    public int QuestionCount
    {
        get => _questionCount;
        set => SetProperty(ref _questionCount, value);
    }

    public bool CanStartTest
    {
        get => _canStartTest;
        set => SetProperty(ref _canStartTest, value);
    }

    public ObservableCollection<TestAttempt> PreviousAttempts
    {
        get => _previousAttempts;
        set => SetProperty(ref _previousAttempts, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public ICommand StartTestCommand { get; }
    public ICommand BackCommand { get; }

    public TestDetailViewModel(
        ITestService testService,
        ITestAttemptService testAttemptService,
        AuthStore authStore,
        SelectedTestStore selectedTestStore,
        INavigationService navigationService)
    {
        _testService = testService;
        _testAttemptService = testAttemptService;
        _authStore = authStore;
        _selectedTestStore = selectedTestStore;
        _navigationService = navigationService;

        StartTestCommand = new RelayCommand(async () => await StartTestAsync(), () => CanStartTest);
        BackCommand = new RelayCommand(GoBack);

        _ = LoadTestDetailsAsync();
    }

    private async Task LoadTestDetailsAsync()
    {
        try
        {
            IsLoading = true;

            if (_selectedTestStore.SelectedTest == null)
            {
                MessageBox.Show("Không tìm thấy thông tin bài thi.", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                GoBack();
                return;
            }

            // Load full test details
            CurrentTest = await _testService.GetTestByIdAsync(_selectedTestStore.SelectedTest.TestId);
            
            if (CurrentTest == null)
            {
                MessageBox.Show("Không tìm thấy thông tin bài thi.", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                GoBack();
                return;
            }

            // Get question count
            QuestionCount = await _testService.GetTestQuestionCountAsync(CurrentTest.TestId);

            // Check if test is available
            var now = DateTime.Now;
            CanStartTest = CurrentTest.IsActive &&
                          (!CurrentTest.AvailableFrom.HasValue || CurrentTest.AvailableFrom <= now) &&
                          (!CurrentTest.AvailableTo.HasValue || CurrentTest.AvailableTo >= now);

            // Load previous attempts
            if (_authStore.CurrentUser != null)
            {
                var attempts = await _testAttemptService.GetStudentAttemptsAsync(
                    _authStore.CurrentUser.UserId);
                
                var testAttempts = attempts.Where(a => a.TestId == CurrentTest.TestId).ToList();
                PreviousAttempts = new ObservableCollection<TestAttempt>(testAttempts);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi tải thông tin bài thi: {ex.Message}", "Lỗi",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task StartTestAsync()
    {
        try
        {
            if (CurrentTest == null || _authStore.CurrentUser == null)
                return;

            var result = MessageBox.Show(
                $"Bạn có chắc chắn muốn bắt đầu bài thi?\n\nThời gian: {CurrentTest.DurationMinutes} phút\nSố câu hỏi: {QuestionCount}",
                "Xác nhận",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _navigationService.NavigateTo<TakeTestViewModel>();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi bắt đầu bài thi: {ex.Message}", "Lỗi",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void GoBack()
    {
        _navigationService.NavigateTo<StudentTestListViewModel>();
    }
}

