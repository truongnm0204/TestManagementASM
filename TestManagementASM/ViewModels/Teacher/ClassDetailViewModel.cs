using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TestManagementASM.Commands;
using TestManagementASM.Models;
using TestManagementASM.Services.Interfaces;
using TestManagementASM.Stores;
using TestManagementASM.ViewModels.Base;

namespace TestManagementASM.ViewModels.Teacher;

public class ClassDetailViewModel : ViewModelBase
{
    private readonly IClassService _classService;
    private readonly ITestAttemptService _testAttemptService;
    private readonly SelectedClassStore _selectedClassStore;
    private readonly SelectedStudentStore _selectedStudentStore;
    private readonly INavigationService _navigationService;

    private Class? _currentClass;
    private ObservableCollection<StudentScoreInfo> _studentScores = new();
    private StudentScoreInfo? _selectedStudent;
    private bool _isLoading;

    public Class? CurrentClass
    {
        get => _currentClass;
        set => SetProperty(ref _currentClass, value);
    }

    public ObservableCollection<StudentScoreInfo> StudentScores
    {
        get => _studentScores;
        set => SetProperty(ref _studentScores, value);
    }

    public StudentScoreInfo? SelectedStudent
    {
        get => _selectedStudent;
        set => SetProperty(ref _selectedStudent, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public ICommand LoadDataCommand { get; }
    public ICommand BackCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand ViewStudentDetailCommand { get; }

    public ClassDetailViewModel(
        IClassService classService,
        ITestAttemptService testAttemptService,
        SelectedClassStore selectedClassStore,
        SelectedStudentStore selectedStudentStore,
        INavigationService navigationService)
    {
        _classService = classService;
        _testAttemptService = testAttemptService;
        _selectedClassStore = selectedClassStore;
        _selectedStudentStore = selectedStudentStore;
        _navigationService = navigationService;

        LoadDataCommand = new RelayCommand(async () => await LoadDataAsync());
        BackCommand = new RelayCommand(Back);
        RefreshCommand = new RelayCommand(async () => await LoadDataAsync());
        ViewStudentDetailCommand = new RelayCommand(param => ViewStudentDetail((StudentScoreInfo)param!));

        _ = LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        try
        {
            IsLoading = true;

            if (_selectedClassStore.SelectedClass == null)
            {
                MessageBox.Show("Không tìm thấy thông tin lớp học.", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Back();
                return;
            }

            // Load class details
            CurrentClass = await _classService.GetClassByIdAsync(_selectedClassStore.SelectedClass.ClassId);

            if (CurrentClass == null)
            {
                MessageBox.Show("Không tìm thấy thông tin lớp học.", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Back();
                return;
            }

            // Load students
            var students = await _classService.GetStudentsByClassAsync(CurrentClass.ClassId);

            // Load test attempts for this class
            var attempts = await _testAttemptService.GetClassTestScoresAsync(CurrentClass.ClassId);

            // Calculate average scores per student
            var studentScoresList = new List<StudentScoreInfo>();

            foreach (var student in students)
            {
                var studentAttempts = attempts
                    .Where(a => a.StudentId == student.UserId && a.AttemptStatus == "Completed")
                    .ToList();

                var averageScore = studentAttempts.Any()
                    ? studentAttempts.Average(a => a.Score ?? 0)
                    : 0;

                var totalTests = CurrentClass.Tests.Count;
                var completedTests = studentAttempts
                    .Select(a => a.TestId)
                    .Distinct()
                    .Count();

                studentScoresList.Add(new StudentScoreInfo
                {
                    StudentId = student.UserId,
                    StudentName = student.FullName,
                    Email = student.Email,
                    AverageScore = Math.Round(averageScore, 2),
                    CompletedTests = completedTests,
                    TotalTests = totalTests,
                    TestAttempts = studentAttempts
                });
            }

            StudentScores = new ObservableCollection<StudentScoreInfo>(
                studentScoresList.OrderBy(s => s.StudentName));
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
        _navigationService.NavigateTo<TeacherClassListViewModel>();
    }

    private void ViewStudentDetail(StudentScoreInfo studentInfo)
    {
        if (studentInfo != null && CurrentClass != null)
        {
            // Create User object for the selected student
            var student = new User
            {
                UserId = studentInfo.StudentId,
                FullName = studentInfo.StudentName,
                Email = studentInfo.Email
            };

            _selectedStudentStore.SelectedStudent = student;
            _selectedStudentStore.SelectedClass = CurrentClass;
            _navigationService.NavigateTo<StudentDetailViewModel>();
        }
    }

    // Helper class for displaying student scores
    public class StudentScoreInfo : ViewModelBase
    {
        private int _studentId;
        private string _studentName = string.Empty;
        private string _email = string.Empty;
        private double _averageScore;
        private int _completedTests;
        private int _totalTests;
        private List<TestAttempt> _testAttempts = new();

        public int StudentId
        {
            get => _studentId;
            set => SetProperty(ref _studentId, value);
        }

        public string StudentName
        {
            get => _studentName;
            set => SetProperty(ref _studentName, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
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

        public List<TestAttempt> TestAttempts
        {
            get => _testAttempts;
            set => SetProperty(ref _testAttempts, value);
        }

        public string CompletionStatus => $"{CompletedTests}/{TotalTests}";
    }
}

