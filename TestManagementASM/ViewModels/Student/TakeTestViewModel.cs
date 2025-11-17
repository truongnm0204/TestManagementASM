using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using TestManagementASM.Models;
using TestManagementASM.Services.Interfaces;
using TestManagementASM.Stores;
using TestManagementASM.ViewModels.Base;

namespace TestManagementASM.ViewModels.Student;

public class TakeTestViewModel : ViewModelBase
{
    private readonly ITestService _testService;
    private readonly ITestAttemptService _testAttemptService;
    private readonly AuthStore _authStore;
    private readonly SelectedTestStore _selectedTestStore;
    private readonly INavigationService _navigationService;
    private readonly DispatcherTimer _timer;

    private Test? _currentTest;
    private TestAttempt? _currentAttempt;
    private ObservableCollection<Question> _questions = new();
    private int _currentQuestionIndex;
    private TimeSpan _remainingTime;
    private Dictionary<int, int> _studentAnswers = new(); // QuestionId -> AnswerId
    private bool _isSubmitting;

    public Test? CurrentTest
    {
        get => _currentTest;
        set => SetProperty(ref _currentTest, value);
    }

    public ObservableCollection<Question> Questions
    {
        get => _questions;
        set => SetProperty(ref _questions, value);
    }

    public int CurrentQuestionIndex
    {
        get => _currentQuestionIndex;
        set
        {
            SetProperty(ref _currentQuestionIndex, value);
            OnPropertyChanged(nameof(CurrentQuestion));
            OnPropertyChanged(nameof(QuestionNumber));
            OnPropertyChanged(nameof(CanGoPrevious));
            OnPropertyChanged(nameof(CanGoNext));
        }
    }

    public Question? CurrentQuestion => Questions.Count > 0 && CurrentQuestionIndex >= 0 && CurrentQuestionIndex < Questions.Count
        ? Questions[CurrentQuestionIndex]
        : null;

    public int QuestionNumber => CurrentQuestionIndex + 1;

    public TimeSpan RemainingTime
    {
        get => _remainingTime;
        set
        {
            SetProperty(ref _remainingTime, value);
            OnPropertyChanged(nameof(RemainingTimeText));
        }
    }

    public string RemainingTimeText => $"{(int)RemainingTime.TotalMinutes:D2}:{RemainingTime.Seconds:D2}";

    public bool CanGoPrevious => CurrentQuestionIndex > 0;
    public bool CanGoNext => CurrentQuestionIndex < Questions.Count - 1;

    public bool IsSubmitting
    {
        get => _isSubmitting;
        set => SetProperty(ref _isSubmitting, value);
    }

    public ICommand NextQuestionCommand { get; }
    public ICommand PreviousQuestionCommand { get; }
    public ICommand SelectAnswerCommand { get; }
    public ICommand SubmitTestCommand { get; }

    public TakeTestViewModel(
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

        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _timer.Tick += Timer_Tick;

        NextQuestionCommand = new RelayCommand(NextQuestion, () => CanGoNext);
        PreviousQuestionCommand = new RelayCommand(PreviousQuestion, () => CanGoPrevious);
        SelectAnswerCommand = new RelayCommand(param => SelectAnswer((int)param!));
        SubmitTestCommand = new RelayCommand(async () => await SubmitTestAsync());

        _ = InitializeTestAsync();
    }

    private async Task InitializeTestAsync()
    {
        try
        {
            if (_selectedTestStore.SelectedTest == null || _authStore.CurrentUser == null)
            {
                MessageBox.Show("Không tìm thấy thông tin bài thi.", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                _navigationService.NavigateTo<StudentTestListViewModel>();
                return;
            }

            CurrentTest = await _testService.GetTestByIdAsync(_selectedTestStore.SelectedTest.TestId);

            if (CurrentTest == null)
            {
                MessageBox.Show("Không tìm thấy thông tin bài thi.", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                _navigationService.NavigateTo<StudentTestListViewModel>();
                return;
            }

            // Load questions
            var questions = await _testService.GetTestQuestionsAsync(CurrentTest.TestId);
            Questions = new ObservableCollection<Question>(questions);

            if (Questions.Count == 0)
            {
                MessageBox.Show("Bài thi không có câu hỏi nào.", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                _navigationService.NavigateTo<StudentTestListViewModel>();
                return;
            }

            // Create test attempt
            _currentAttempt = await _testAttemptService.CreateAttemptAsync(
                _authStore.CurrentUser.UserId,
                CurrentTest.TestId);

            // Initialize timer
            RemainingTime = TimeSpan.FromMinutes(CurrentTest.DurationMinutes ?? 60);
            StartTimer();

            CurrentQuestionIndex = 0;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi khởi tạo bài thi: {ex.Message}", "Lỗi",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void StartTimer()
    {
        _timer.Start();
    }

    private void StopTimer()
    {
        _timer.Stop();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        if (RemainingTime.TotalSeconds > 0)
        {
            RemainingTime = RemainingTime.Subtract(TimeSpan.FromSeconds(1));
        }
        else
        {
            // Time's up - auto submit
            StopTimer();
            MessageBox.Show("Hết thời gian làm bài! Bài thi sẽ được nộp tự động.", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
            _ = SubmitTestAsync();
        }
    }

    private void NextQuestion()
    {
        if (CanGoNext)
        {
            CurrentQuestionIndex++;
        }
    }

    private void PreviousQuestion()
    {
        if (CanGoPrevious)
        {
            CurrentQuestionIndex--;
        }
    }

    private async void SelectAnswer(int answerId)
    {
        if (CurrentQuestion == null || _currentAttempt == null)
            return;

        try
        {
            // Save answer locally
            _studentAnswers[CurrentQuestion.QuestionId] = answerId;

            // Save to database
            await _testAttemptService.SaveStudentAnswerAsync(
                _currentAttempt.AttemptId,
                CurrentQuestion.QuestionId,
                answerId);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi lưu câu trả lời: {ex.Message}", "Lỗi",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task SubmitTestAsync()
    {
        if (IsSubmitting || _currentAttempt == null)
            return;

        try
        {
            IsSubmitting = true;
            StopTimer();

            var result = MessageBox.Show(
                $"Bạn đã trả lời {_studentAnswers.Count}/{Questions.Count} câu hỏi.\n\nBạn có chắc chắn muốn nộp bài?",
                "Xác nhận nộp bài",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Complete the attempt (calculate score)
                bool success = await _testAttemptService.CompleteAttemptAsync(_currentAttempt.AttemptId);

                if (success)
                {
                    // Get the updated attempt with score
                    var completedAttempt = await _testAttemptService.GetAttemptByIdAsync(_currentAttempt.AttemptId);

                    MessageBox.Show(
                        $"Nộp bài thành công!\n\nĐiểm của bạn: {completedAttempt?.Score ?? 0}/100",
                        "Hoàn thành",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    _navigationService.NavigateTo<StudentTestListViewModel>();
                }
                else
                {
                    MessageBox.Show("Có lỗi khi nộp bài. Vui lòng thử lại.", "Lỗi",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    StartTimer(); // Resume timer
                }
            }
            else
            {
                StartTimer(); // Resume timer
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi nộp bài: {ex.Message}", "Lỗi",
                MessageBoxButton.OK, MessageBoxImage.Error);
            StartTimer(); // Resume timer
        }
        finally
        {
            IsSubmitting = false;
        }
    }

    public int GetSelectedAnswerId(int questionId)
    {
        return _studentAnswers.TryGetValue(questionId, out int answerId) ? answerId : 0;
    }
}
