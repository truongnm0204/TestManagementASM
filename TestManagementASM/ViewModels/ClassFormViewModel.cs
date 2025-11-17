using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TestManagementASM.Commands;
using TestManagementASM.Models;
using TestManagementASM.Services.Interfaces;
using TestManagementASM.ViewModels.Base;

namespace TestManagementASM.ViewModels;

public class ClassFormViewModel : ViewModelBase
{
    private readonly IClassService _classService;
    private readonly ISubjectService _subjectService;
    private Class _class = new();
    private ObservableCollection<Subject> _subjects = new();
    private Subject? _selectedSubject;
    private bool _isEditMode;
    private bool _isLoading;
    private string _errorMessage = string.Empty;

    public Class Class
    {
        get => _class;
        set => SetProperty(ref _class, value);
    }

    public ObservableCollection<Subject> Subjects
    {
        get => _subjects;
        set => SetProperty(ref _subjects, value);
    }

    public Subject? SelectedSubject
    {
        get => _selectedSubject;
        set
        {
            SetProperty(ref _selectedSubject, value);
            if (value != null)
                Class.SubjectId = value.SubjectId;
        }
    }

    public bool IsEditMode
    {
        get => _isEditMode;
        set => SetProperty(ref _isEditMode, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public ClassFormViewModel(IClassService classService, ISubjectService subjectService)
    {
        _classService = classService;
        _subjectService = subjectService;
        SaveCommand = new RelayCommand(async () => await SaveAsync());
        CancelCommand = new RelayCommand(() => OnClosed?.Invoke());

        _ = LoadSubjectsAsync();
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
            ErrorMessage = $"Lỗi khi tải môn học: {ex.Message}";
        }
    }

    public void InitializeForCreate()
    {
        IsEditMode = false;
        Class = new Class();
        SelectedSubject = null;
        ErrorMessage = string.Empty;
    }

    public void InitializeForEdit(Class @class)
    {
        IsEditMode = true;
        Class = new Class
        {
            ClassId = @class.ClassId,
            ClassName = @class.ClassName,
            SubjectId = @class.SubjectId,
            Semester = @class.Semester
        };
        SelectedSubject = Subjects.FirstOrDefault(s => s.SubjectId == @class.SubjectId);
        ErrorMessage = string.Empty;
    }

    private async Task SaveAsync()
    {
        try
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Class.ClassName))
            {
                ErrorMessage = "Tên lớp không được để trống!";
                return;
            }

            if (SelectedSubject == null)
            {
                ErrorMessage = "Vui lòng chọn môn học!";
                return;
            }

            if (IsEditMode)
            {
                await _classService.UpdateClassAsync(Class);
                MessageBox.Show("Cập nhật lớp thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                await _classService.AddClassAsync(Class);
                MessageBox.Show("Tạo lớp thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            OnClosed?.Invoke();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Lỗi: {ex.Message}";
        }
    }

    public event Action? OnClosed;
}

