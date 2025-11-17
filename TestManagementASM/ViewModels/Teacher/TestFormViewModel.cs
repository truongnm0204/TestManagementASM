using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TestManagementASM.Commands;
using TestManagementASM.Models;
using TestManagementASM.Services.Interfaces;
using TestManagementASM.Stores;
using TestManagementASM.ViewModels.Base;

namespace TestManagementASM.ViewModels.Teacher;

public class TestFormViewModel : ViewModelBase
{
    private readonly ITestService _testService;
    private readonly IClassService _classService;
    private readonly IQuestionService _questionService;
    private readonly ISubjectService _subjectService;
    private readonly AuthStore _authStore;
    private readonly SelectedTestStore _selectedTestStore;
    private readonly INavigationService _navigationService;

    private Test? _currentTest;
    private ObservableCollection<Class> _teacherClasses = new();
    private ObservableCollection<Subject> _subjects = new();
    private ObservableCollection<Question> _availableQuestions = new();
    private ObservableCollection<QuestionItem> _selectedQuestions = new();
    private Class? _selectedClass;
    private Subject? _selectedSubject;
    private string _testName = string.Empty;
    private int _durationMinutes = 60;
    private bool _isActive = true;
    private DateTime? _availableFrom;
    private DateTime? _availableTo;
    private bool _isEditMode;
    private bool _isSaving;
    private bool _isLoadingQuestions;
    private bool _isLoadingClasses;

    public Test? CurrentTest
    {
        get => _currentTest;
        set => SetProperty(ref _currentTest, value);
    }

    public ObservableCollection<Class> TeacherClasses
    {
        get => _teacherClasses;
        set => SetProperty(ref _teacherClasses, value);
    }

    public ObservableCollection<Subject> Subjects
    {
        get => _subjects;
        set => SetProperty(ref _subjects, value);
    }

    public ObservableCollection<Question> AvailableQuestions
    {
        get => _availableQuestions;
        set => SetProperty(ref _availableQuestions, value);
    }

    public ObservableCollection<QuestionItem> SelectedQuestions
    {
        get => _selectedQuestions;
        set
        {
            if (_selectedQuestions != null)
            {
                _selectedQuestions.CollectionChanged -= OnSelectedQuestionsChanged;
            }
            SetProperty(ref _selectedQuestions, value);
            if (_selectedQuestions != null)
            {
                _selectedQuestions.CollectionChanged += OnSelectedQuestionsChanged;
            }
            OnPropertyChanged(nameof(TotalPoints));
        }
    }

    public int TotalPoints => SelectedQuestions.Sum(q => q.Points);

    public Class? SelectedClass
    {
        get => _selectedClass;
        set => SetProperty(ref _selectedClass, value);
    }

    public Subject? SelectedSubject
    {
        get => _selectedSubject;
        set
        {
            SetProperty(ref _selectedSubject, value);
            _ = LoadQuestionsAsync();
        }
    }

    public string TestName
    {
        get => _testName;
        set => SetProperty(ref _testName, value);
    }

    public int DurationMinutes
    {
        get => _durationMinutes;
        set => SetProperty(ref _durationMinutes, value);
    }

    public bool IsActive
    {
        get => _isActive;
        set => SetProperty(ref _isActive, value);
    }

    public DateTime? AvailableFrom
    {
        get => _availableFrom;
        set => SetProperty(ref _availableFrom, value);
    }

    public DateTime? AvailableTo
    {
        get => _availableTo;
        set => SetProperty(ref _availableTo, value);
    }

    public bool IsEditMode
    {
        get => _isEditMode;
        set => SetProperty(ref _isEditMode, value);
    }

    public bool IsSaving
    {
        get => _isSaving;
        set => SetProperty(ref _isSaving, value);
    }

    public bool IsLoadingQuestions
    {
        get => _isLoadingQuestions;
        set => SetProperty(ref _isLoadingQuestions, value);
    }

    public bool IsLoadingClasses
    {
        get => _isLoadingClasses;
        set => SetProperty(ref _isLoadingClasses, value);
    }

    public ICommand SaveTestCommand { get; }
    public ICommand CancelCommand { get; }
    public ICommand AddQuestionCommand { get; }
    public ICommand RemoveQuestionCommand { get; }
    public ICommand LoadQuestionsCommand { get; }

    public TestFormViewModel(
        ITestService testService,
        IClassService classService,
        IQuestionService questionService,
        ISubjectService subjectService,
        AuthStore authStore,
        SelectedTestStore selectedTestStore,
        INavigationService navigationService)
    {
        _testService = testService;
        _classService = classService;
        _questionService = questionService;
        _subjectService = subjectService;
        _authStore = authStore;
        _selectedTestStore = selectedTestStore;
        _navigationService = navigationService;

        SaveTestCommand = new RelayCommand(async () => await SaveTestAsync(), CanSave);
        CancelCommand = new RelayCommand(Cancel);
        AddQuestionCommand = new RelayCommand(param => AddQuestion((Question)param!), param => param is Question);
        RemoveQuestionCommand = new RelayCommand(param => RemoveQuestion((QuestionItem)param!), param => param is QuestionItem);
        LoadQuestionsCommand = new RelayCommand(async () => await LoadQuestionsAsync());

        _ = LoadClassesAsync();
        _ = LoadSubjectsAsync();

        if (_selectedTestStore.SelectedTest != null)
        {
            IsEditMode = true;
            _ = LoadTestForEditAsync(_selectedTestStore.SelectedTest.TestId);
        }

        SelectedQuestions.CollectionChanged += OnSelectedQuestionsChanged;
    }

    private void OnSelectedQuestionsChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(TotalPoints));

        // Subscribe to Points property changes for new items
        if (e.NewItems != null)
        {
            foreach (QuestionItem item in e.NewItems)
            {
                item.PropertyChanged += (s, args) =>
                {
                    if (args.PropertyName == nameof(QuestionItem.Points))
                    {
                        OnPropertyChanged(nameof(TotalPoints));
                    }
                };
            }
        }
    }

    private async Task LoadClassesAsync()
    {
        try
        {
            IsLoadingClasses = true;

            if (_authStore.CurrentUser == null)
            {
                MessageBox.Show("Vui lòng đăng nhập để tạo bài thi.", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var classes = await _classService.GetTeacherClassesAsync(_authStore.CurrentUser.UserId);
            TeacherClasses = new ObservableCollection<Class>(classes);

            if (TeacherClasses.Count == 0)
            {
                MessageBox.Show("Bạn chưa được phân công giảng dạy lớp nào.\nVui lòng liên hệ quản trị viên để được phân công lớp.",
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi tải danh sách lớp: {ex.Message}\n\nStackTrace: {ex.StackTrace}", "Lỗi",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsLoadingClasses = false;
        }
    }

    private async Task LoadSubjectsAsync()
    {
        try
        {
            var subjects = await _subjectService.GetAllSubjectsAsync();
            Subjects = new ObservableCollection<Subject>(subjects.Where(s => s.Status));
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi tải danh sách môn học: {ex.Message}", "Lỗi",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task LoadQuestionsAsync()
    {
        if (SelectedSubject == null) return;

        try
        {
            IsLoadingQuestions = true;
            var questions = await _questionService.GetQuestionsBySubjectAsync(SelectedSubject.SubjectId);

            // Filter out already selected questions
            var selectedQuestionIds = SelectedQuestions.Select(q => q.QuestionId).ToHashSet();
            var availableQuestions = questions.Where(q => !selectedQuestionIds.Contains(q.QuestionId)).ToList();

            AvailableQuestions = new ObservableCollection<Question>(availableQuestions);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi tải danh sách câu hỏi: {ex.Message}", "Lỗi",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsLoadingQuestions = false;
        }
    }

    private async Task LoadTestForEditAsync(int testId)
    {
        try
        {
            var test = await _testService.GetTestByIdAsync(testId);
            if (test != null)
            {
                CurrentTest = test;
                TestName = test.TestName;
                DurationMinutes = test.DurationMinutes ?? 60;
                IsActive = test.IsActive;
                AvailableFrom = test.AvailableFrom;
                AvailableTo = test.AvailableTo;
                SelectedClass = TeacherClasses.FirstOrDefault(c => c.ClassId == test.ClassId);

                // Load selected questions
                SelectedQuestions.Clear();
                foreach (var tq in test.TestQuestions)
                {
                    SelectedQuestions.Add(new QuestionItem
                    {
                        QuestionId = tq.QuestionId,
                        QuestionText = tq.Question.QuestionText,
                        QuestionType = tq.Question.QuestionType,
                        DifficultyLevel = tq.Question.DifficultyLevel ?? 1,
                        Points = tq.Points
                    });
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi tải thông tin bài thi: {ex.Message}", "Lỗi",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private bool CanSave()
    {
        return !string.IsNullOrWhiteSpace(TestName) &&
               SelectedClass != null &&
               SelectedQuestions.Count > 0 &&
               DurationMinutes > 0;
    }

    private async Task SaveTestAsync()
    {
        if (!CanSave()) return;

        try
        {
            IsSaving = true;

            var test = new Test
            {
                TestId = CurrentTest?.TestId ?? 0,
                ClassId = SelectedClass!.ClassId,
                CreatedByTeacherId = _authStore.CurrentUser!.UserId,
                TestName = TestName,
                DurationMinutes = DurationMinutes,
                IsActive = IsActive,
                AvailableFrom = AvailableFrom,
                AvailableTo = AvailableTo
            };

            var testQuestions = SelectedQuestions.Select(q => new TestQuestion
            {
                QuestionId = q.QuestionId,
                Points = q.Points
            }).ToList();

            bool success;
            if (IsEditMode)
            {
                // For edit mode, we need to update test and questions separately
                success = await _testService.UpdateTestAsync(test);
                // TODO: Add method to update test questions
            }
            else
            {
                success = await _testService.CreateTestAsync(test, testQuestions);
            }

            if (success)
            {
                MessageBox.Show(
                    IsEditMode ? "Cập nhật bài thi thành công!" : "Tạo bài thi thành công!",
                    "Thành công",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                _navigationService.NavigateTo<TeacherTestListViewModel>();
            }
            else
            {
                MessageBox.Show("Không thể lưu bài thi. Vui lòng kiểm tra lại thông tin!", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi lưu bài thi: {ex.Message}", "Lỗi",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsSaving = false;
        }
    }

    private void Cancel()
    {
        _navigationService.NavigateTo<TeacherTestListViewModel>();
    }

    private void AddQuestion(Question question)
    {
        if (question == null) return;

        // Add to selected questions
        SelectedQuestions.Add(new QuestionItem
        {
            QuestionId = question.QuestionId,
            QuestionText = question.QuestionText,
            QuestionType = question.QuestionType,
            DifficultyLevel = question.DifficultyLevel ?? 1,
            Points = 1 // Default points
        });

        // Remove from available questions
        var questionToRemove = AvailableQuestions.FirstOrDefault(q => q.QuestionId == question.QuestionId);
        if (questionToRemove != null)
        {
            AvailableQuestions.Remove(questionToRemove);
        }
    }

    private void RemoveQuestion(QuestionItem questionItem)
    {
        if (questionItem == null) return;

        SelectedQuestions.Remove(questionItem);

        // Reload available questions to include the removed question
        _ = LoadQuestionsAsync();
    }

    // Helper class for binding selected questions with points
    public class QuestionItem : ViewModelBase
    {
        private int _questionId;
        private string _questionText = string.Empty;
        private string _questionType = string.Empty;
        private int _difficultyLevel;
        private int _points = 1;

        public int QuestionId
        {
            get => _questionId;
            set => SetProperty(ref _questionId, value);
        }

        public string QuestionText
        {
            get => _questionText;
            set => SetProperty(ref _questionText, value);
        }

        public string QuestionType
        {
            get => _questionType;
            set => SetProperty(ref _questionType, value);
        }

        public int DifficultyLevel
        {
            get => _difficultyLevel;
            set => SetProperty(ref _difficultyLevel, value);
        }

        public int Points
        {
            get => _points;
            set => SetProperty(ref _points, value);
        }
    }
}

