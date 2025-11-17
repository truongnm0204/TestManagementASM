using System.Windows;
using System.Windows.Input;
using TestManagementASM.Commands;
using TestManagementASM.Models;
using TestManagementASM.Services.Interfaces;
using TestManagementASM.ViewModels.Base;

namespace TestManagementASM.ViewModels;

public class SubjectFormViewModel : ViewModelBase
{
    private readonly ISubjectService _subjectService;
    private Subject _subject = new();
    private bool _isEditMode;
    private bool _isLoading;
    private string _errorMessage = string.Empty;

    public Subject Subject
    {
        get => _subject;
        set => SetProperty(ref _subject, value);
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

    public SubjectFormViewModel(ISubjectService subjectService)
    {
        _subjectService = subjectService;
        SaveCommand = new RelayCommand(async () => await SaveAsync());
        CancelCommand = new RelayCommand(() => OnClosed?.Invoke());
    }

    public void InitializeForCreate()
    {
        IsEditMode = false;
        Subject = new Subject { Status = true };
        ErrorMessage = string.Empty;
    }

    public void InitializeForEdit(Subject subject)
    {
        IsEditMode = true;
        Subject = new Subject
        {
            SubjectId = subject.SubjectId,
            SubjectCode = subject.SubjectCode,
            SubjectName = subject.SubjectName,
            Status = subject.Status
        };
        ErrorMessage = string.Empty;
    }

    private async Task SaveAsync()
    {
        try
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Subject.SubjectCode))
            {
                ErrorMessage = "Mã môn học không được để trống!";
                return;
            }

            if (string.IsNullOrWhiteSpace(Subject.SubjectName))
            {
                ErrorMessage = "Tên môn học không được để trống!";
                return;
            }

            if (IsEditMode)
            {
                var isUnique = await _subjectService.IsSubjectCodeUniqueAsync(Subject.SubjectCode, Subject.SubjectId);
                if (!isUnique)
                {
                    ErrorMessage = "Mã môn học đã tồn tại!";
                    return;
                }

                await _subjectService.UpdateSubjectAsync(Subject);
                MessageBox.Show("Cập nhật môn học thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                var isUnique = await _subjectService.IsSubjectCodeUniqueAsync(Subject.SubjectCode);
                if (!isUnique)
                {
                    ErrorMessage = "Mã môn học đã tồn tại!";
                    return;
                }

                await _subjectService.AddSubjectAsync(Subject);
                MessageBox.Show("Tạo môn học thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
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

