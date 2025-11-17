using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TestManagementASM.Commands;
using TestManagementASM.Models;
using TestManagementASM.Services.Interfaces;
using TestManagementASM.Stores;
using TestManagementASM.ViewModels.Base;

namespace TestManagementASM.ViewModels.Teacher;

public class QuestionFormViewModel : ViewModelBase
{
    private readonly IQuestionService _questionService;
    private readonly ISubjectService _subjectService;
    private readonly AuthStore _authStore;
    private readonly SelectedQuestionStore _selectedQuestionStore;
    private readonly INavigationService _navigationService;

    private Question? _currentQuestion;
    private ObservableCollection<AnswerItem> _answers = new();
    private ObservableCollection<Subject> _subjects = new();
    private Subject? _selectedSubject;
    private string _questionText = string.Empty;
    private string _selectedQuestionType = "SINGLE";
    private int _difficultyLevel = 1;
    private int _chapter = 1;
    private bool _isEditMode;
    private bool _isSaving;

    public Question? CurrentQuestion
    {
        get => _currentQuestion;
        set => SetProperty(ref _currentQuestion, value);
    }

    public ObservableCollection<AnswerItem> Answers
    {
        get => _answers;
        set => SetProperty(ref _answers, value);
    }

    public ObservableCollection<Subject> Subjects
    {
        get => _subjects;
        set => SetProperty(ref _subjects, value);
    }

    public Subject? SelectedSubject
    {
        get => _selectedSubject;
        set => SetProperty(ref _selectedSubject, value);
    }

    public string QuestionText
    {
        get => _questionText;
        set => SetProperty(ref _questionText, value);
    }

    public string SelectedQuestionType
    {
        get => _selectedQuestionType;
        set => SetProperty(ref _selectedQuestionType, value);
    }

    public int DifficultyLevel
    {
        get => _difficultyLevel;
        set => SetProperty(ref _difficultyLevel, value);
    }

    public int Chapter
    {
        get => _chapter;
        set => SetProperty(ref _chapter, value);
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

    public List<string> QuestionTypes { get; } = new() { "SINGLE", "MULTIPLE" };
    public List<int> DifficultyLevels { get; } = new() { 1, 2, 3, 4, 5 };

    public ICommand SaveQuestionCommand { get; }
    public ICommand CancelCommand { get; }
    public ICommand AddAnswerCommand { get; }
    public ICommand RemoveAnswerCommand { get; }

    public QuestionFormViewModel(
        IQuestionService questionService,
        ISubjectService subjectService,
        AuthStore authStore,
        SelectedQuestionStore selectedQuestionStore,
        INavigationService navigationService)
    {
        _questionService = questionService;
        _subjectService = subjectService;
        _authStore = authStore;
        _selectedQuestionStore = selectedQuestionStore;
        _navigationService = navigationService;

        SaveQuestionCommand = new RelayCommand(async () => await SaveQuestionAsync(), CanSave);
        CancelCommand = new RelayCommand(Cancel);
        AddAnswerCommand = new RelayCommand(AddAnswer);
        RemoveAnswerCommand = new RelayCommand(param => RemoveAnswer((AnswerItem)param!), param => Answers.Count > 2);

        _ = LoadSubjectsAsync();

        if (_selectedQuestionStore.SelectedQuestion != null)
        {
            IsEditMode = true;
            _ = LoadQuestionForEditAsync(_selectedQuestionStore.SelectedQuestion.QuestionId);
        }
        else
        {
            // Initialize with 4 empty answers for new question
            for (int i = 0; i < 4; i++)
            {
                Answers.Add(new AnswerItem { AnswerText = string.Empty, IsCorrect = false });
            }
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

    private async Task LoadQuestionForEditAsync(int questionId)
    {
        try
        {
            var question = await _questionService.GetQuestionByIdAsync(questionId);
            if (question != null)
            {
                CurrentQuestion = question;
                QuestionText = question.QuestionText;
                SelectedQuestionType = question.QuestionType;
                DifficultyLevel = question.DifficultyLevel ?? 1;
                Chapter = question.Chapter ?? 1;
                SelectedSubject = Subjects.FirstOrDefault(s => s.SubjectId == question.SubjectId);

                // Load answers
                Answers.Clear();
                foreach (var answer in question.Answers)
                {
                    Answers.Add(new AnswerItem
                    {
                        AnswerId = answer.AnswerId,
                        AnswerText = answer.AnswerText,
                        IsCorrect = answer.IsCorrect,
                        Feedback = answer.Feedback
                    });
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi tải câu hỏi: {ex.Message}", "Lỗi",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private bool CanSave()
    {
        return !string.IsNullOrWhiteSpace(QuestionText) &&
               SelectedSubject != null &&
               Answers.Count >= 2 &&
               Answers.Any(a => !string.IsNullOrWhiteSpace(a.AnswerText)) &&
               Answers.Any(a => a.IsCorrect);
    }

    private async Task SaveQuestionAsync()
    {
        try
        {
            // Validate at least one correct answer
            if (!Answers.Any(a => a.IsCorrect))
            {
                MessageBox.Show("Phải có ít nhất một đáp án đúng!", "Cảnh báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validate all answers have text
            var validAnswers = Answers.Where(a => !string.IsNullOrWhiteSpace(a.AnswerText)).ToList();
            if (validAnswers.Count < 2)
            {
                MessageBox.Show("Phải có ít nhất 2 đáp án!", "Cảnh báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            IsSaving = true;

            var question = new Question
            {
                QuestionId = CurrentQuestion?.QuestionId ?? 0,
                SubjectId = SelectedSubject!.SubjectId,
                CreatedByTeacherId = _authStore.CurrentUser?.UserId,
                QuestionText = QuestionText,
                QuestionType = SelectedQuestionType,
                DifficultyLevel = DifficultyLevel,
                Chapter = Chapter
            };

            var answers = validAnswers.Select(a => new Answer
            {
                AnswerId = a.AnswerId,
                AnswerText = a.AnswerText,
                IsCorrect = a.IsCorrect,
                Feedback = a.Feedback
            }).ToList();

            bool success;
            if (IsEditMode)
            {
                success = await _questionService.UpdateQuestionAsync(question, answers);
            }
            else
            {
                success = await _questionService.CreateQuestionAsync(question, answers);
            }

            if (success)
            {
                MessageBox.Show(
                    IsEditMode ? "Cập nhật câu hỏi thành công!" : "Thêm câu hỏi thành công!",
                    "Thành công",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                _navigationService.NavigateTo<QuestionListViewModel>();
            }
            else
            {
                MessageBox.Show("Không thể lưu câu hỏi. Vui lòng thử lại!", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi lưu câu hỏi: {ex.Message}", "Lỗi",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsSaving = false;
        }
    }

    private void Cancel()
    {
        _navigationService.NavigateTo<QuestionListViewModel>();
    }

    private void AddAnswer()
    {
        Answers.Add(new AnswerItem { AnswerText = string.Empty, IsCorrect = false });
    }

    private void RemoveAnswer(AnswerItem answer)
    {
        if (Answers.Count > 2)
        {
            Answers.Remove(answer);
        }
    }
}

// Helper class for binding answers in UI
public class AnswerItem : ViewModelBase
{
    private int _answerId;
    private string _answerText = string.Empty;
    private bool _isCorrect;
    private string? _feedback;

    public int AnswerId
    {
        get => _answerId;
        set => SetProperty(ref _answerId, value);
    }

    public string AnswerText
    {
        get => _answerText;
        set => SetProperty(ref _answerText, value);
    }

    public bool IsCorrect
    {
        get => _isCorrect;
        set => SetProperty(ref _isCorrect, value);
    }

    public string? Feedback
    {
        get => _feedback;
        set => SetProperty(ref _feedback, value);
    }
}

