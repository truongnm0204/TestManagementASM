using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TestManagementASM.Commands;
using TestManagementASM.Models;
using TestManagementASM.Services.Interfaces;
using TestManagementASM.Stores;
using TestManagementASM.ViewModels.Base;

namespace TestManagementASM.ViewModels.Teacher;

public class StudentDetailViewModel : ViewModelBase
{
    private readonly ITestAttemptService _testAttemptService;
    private readonly SelectedStudentStore _selectedStudentStore;
    private readonly INavigationService _navigationService;

    private User? _currentStudent;
    private Class? _currentClass;
    private ObservableCollection<TestAttemptInfo> _testAttempts = new();
    private double _averageScore;
    private int _completedTests;
    private int _totalTests;
    private bool _isLoading;

    // Wrapper class to hold TestAttempt with attempt number
    public class TestAttemptInfo
    {
        public TestAttempt Attempt { get; set; } = null!;
        public int AttemptNumber { get; set; }
        public string AttemptLabel => $"Lần {AttemptNumber}";
    }

    public User? CurrentStudent
    {
        get => _currentStudent;
        set => SetProperty(ref _currentStudent, value);
    }

    public Class? CurrentClass
    {
        get => _currentClass;
        set => SetProperty(ref _currentClass, value);
    }

    public ObservableCollection<TestAttemptInfo> TestAttempts
    {
        get => _testAttempts;
        set => SetProperty(ref _testAttempts, value);
    }

    public double AverageScore
    {
        get => _averageScore;
        set => SetProperty(ref _averageScore, value);
    }

    public int CompletedTests
    {
        get => _completedTests;
        set => SetProperty(ref _completedTests, value);
    }

    public int TotalTests
    {
        get => _totalTests;
        set => SetProperty(ref _totalTests, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public ICommand LoadAttemptsCommand { get; }
    public ICommand BackCommand { get; }
    public ICommand RefreshCommand { get; }

    public StudentDetailViewModel(
        ITestAttemptService testAttemptService,
        SelectedStudentStore selectedStudentStore,
        INavigationService navigationService)
    {
        _testAttemptService = testAttemptService;
        _selectedStudentStore = selectedStudentStore;
        _navigationService = navigationService;

        LoadAttemptsCommand = new RelayCommand(async () => await LoadAttemptsAsync());
        BackCommand = new RelayCommand(Back);
        RefreshCommand = new RelayCommand(async () => await LoadAttemptsAsync());

        _ = LoadAttemptsAsync();
    }

    private async Task LoadAttemptsAsync()
    {
        try
        {
            IsLoading = true;

            if (_selectedStudentStore.SelectedStudent == null || _selectedStudentStore.SelectedClass == null)
            {
                MessageBox.Show("Không tìm thấy thông tin sinh viên.", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Back();
                return;
            }

            CurrentStudent = _selectedStudentStore.SelectedStudent;
            CurrentClass = _selectedStudentStore.SelectedClass;

            // Load test attempts for this student in this class
            var attempts = await _testAttemptService.GetStudentAttemptsAsync(
                CurrentStudent.UserId,
                CurrentClass.ClassId);

            // Group by TestId and calculate attempt number for each test
            var attemptInfos = attempts
                .OrderBy(a => a.StartTime) // Sort by time first
                .GroupBy(a => a.TestId)
                .SelectMany(group =>
                {
                    int attemptNumber = 1;
                    return group.Select(attempt => new TestAttemptInfo
                    {
                        Attempt = attempt,
                        AttemptNumber = attemptNumber++
                    });
                })
                .OrderByDescending(info => info.Attempt.StartTime) // Then sort by most recent
                .ToList();

            TestAttempts = new ObservableCollection<TestAttemptInfo>(attemptInfos);

            // Calculate statistics
            var completedAttempts = attempts
                .Where(a => a.AttemptStatus == "Completed")
                .ToList();

            CompletedTests = completedAttempts
                .Select(a => a.TestId)
                .Distinct()
                .Count();

            TotalTests = CurrentClass.Tests.Count;

            AverageScore = completedAttempts.Any()
                ? Math.Round(completedAttempts.Average(a => a.Score ?? 0), 2)
                : 0;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void Back()
    {
        _navigationService.NavigateTo<ClassDetailViewModel>();
    }
}

