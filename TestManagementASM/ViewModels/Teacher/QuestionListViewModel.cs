using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TestManagementASM.Commands;
using TestManagementASM.Models;
using TestManagementASM.Services.Interfaces;
using TestManagementASM.Stores;
using TestManagementASM.ViewModels.Base;

namespace TestManagementASM.ViewModels.Teacher;

public class QuestionListViewModel : ViewModelBase
{
    private readonly IQuestionService _questionService;
    private readonly ISubjectService _subjectService;
    private readonly AuthStore _authStore;
    private readonly SelectedQuestionStore _selectedQuestionStore;
    private readonly INavigationService _navigationService;

    private ObservableCollection<Question> _questions = new();
    private ObservableCollection<Subject> _subjects = new();
    private Question? _selectedQuestion;
    private Subject? _selectedSubject;
    private string _searchText = string.Empty;
    private bool _isLoading;

    public ObservableCollection<Question> Questions
    {
        get => _questions;
        set => SetProperty(ref _questions, value);
    }

    public ObservableCollection<Subject> Subjects
    {
        get => _subjects;
        set => SetProperty(ref _subjects, value);
    }

    public Question? SelectedQuestion
    {
        get => _selectedQuestion;
        set => SetProperty(ref _selectedQuestion, value);
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

    public string SearchText
    {
        get => _searchText;
        set
        {
            SetProperty(ref _searchText, value);
            _ = LoadQuestionsAsync();
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public ICommand LoadQuestionsCommand { get; }
    public ICommand AddQuestionCommand { get; }
    public ICommand EditQuestionCommand { get; }
    public ICommand DeleteQuestionCommand { get; }
    public ICommand RefreshCommand { get; }

    public QuestionListViewModel(
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

        LoadQuestionsCommand = new RelayCommand(async () => await LoadQuestionsAsync());
        RefreshCommand = new RelayCommand(async () => await LoadQuestionsAsync());
        AddQuestionCommand = new RelayCommand(AddQuestion);
        EditQuestionCommand = new RelayCommand(EditQuestion, () => SelectedQuestion != null);
        DeleteQuestionCommand = new RelayCommand(async () => await DeleteQuestionAsync(), () => SelectedQuestion != null);

        _ = LoadSubjectsAsync();
        _ = LoadQuestionsAsync();
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
        try
        {
            IsLoading = true;

            if (_authStore.CurrentUser == null)
            {
                MessageBox.Show("Vui lòng đăng nhập để xem danh sách câu hỏi.", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var questions = await _questionService.GetQuestionsByTeacherAsync(_authStore.CurrentUser.UserId);

            // Filter by subject if selected
            if (SelectedSubject != null)
            {
                questions = questions.Where(q => q.SubjectId == SelectedSubject.SubjectId).ToList();
            }

            // Filter by search text
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                questions = questions.Where(q =>
                    q.QuestionText.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    q.Subject.SubjectName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            Questions = new ObservableCollection<Question>(questions);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi tải danh sách câu hỏi: {ex.Message}", "Lỗi",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void AddQuestion()
    {
        _selectedQuestionStore.SelectedQuestion = null;
        _navigationService.NavigateTo<QuestionFormViewModel>();
    }

    private void EditQuestion()
    {
        if (SelectedQuestion != null)
        {
            _selectedQuestionStore.SelectedQuestion = SelectedQuestion;
            _navigationService.NavigateTo<QuestionFormViewModel>();
        }
    }

    private async Task DeleteQuestionAsync()
    {
        if (SelectedQuestion == null) return;

        var result = MessageBox.Show(
            $"Bạn có chắc chắn muốn xóa câu hỏi này?\nThao tác này không thể hoàn tác!",
            "Xác nhận xóa",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            try
            {
                IsLoading = true;
                var success = await _questionService.DeleteQuestionAsync(SelectedQuestion.QuestionId);

                if (success)
                {
                    MessageBox.Show("Xóa câu hỏi thành công!", "Thành công",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    await LoadQuestionsAsync();
                }
                else
                {
                    MessageBox.Show("Không thể xóa câu hỏi. Vui lòng thử lại!", "Lỗi",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa câu hỏi: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}

