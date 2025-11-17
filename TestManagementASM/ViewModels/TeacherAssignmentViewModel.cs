using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TestManagementASM.Commands;
using TestManagementASM.Models;
using TestManagementASM.Services.Interfaces;
using TestManagementASM.ViewModels.Base;

namespace TestManagementASM.ViewModels;

public class TeacherAssignmentViewModel : ViewModelBase
{
    private readonly ITeachingAssignmentService _assignmentService;
    private readonly IUserService _userService;
    private int _classId;
    private ObservableCollection<User> _availableTeachers = new();
    private ObservableCollection<User> _assignedTeachers = new();
    private User? _selectedAvailableTeacher;
    private User? _selectedAssignedTeacher;
    private bool _isLoading;
    private string _errorMessage = string.Empty;

    public ObservableCollection<User> AvailableTeachers
    {
        get => _availableTeachers;
        set => SetProperty(ref _availableTeachers, value);
    }

    public ObservableCollection<User> AssignedTeachers
    {
        get => _assignedTeachers;
        set => SetProperty(ref _assignedTeachers, value);
    }

    public User? SelectedAvailableTeacher
    {
        get => _selectedAvailableTeacher;
        set => SetProperty(ref _selectedAvailableTeacher, value);
    }

    public User? SelectedAssignedTeacher
    {
        get => _selectedAssignedTeacher;
        set => SetProperty(ref _selectedAssignedTeacher, value);
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

    public ICommand AssignCommand { get; }
    public ICommand RemoveCommand { get; }
    public ICommand CloseCommand { get; }

    public TeacherAssignmentViewModel(ITeachingAssignmentService assignmentService, IUserService userService)
    {
        _assignmentService = assignmentService;
        _userService = userService;
        AssignCommand = new RelayCommand(async () => await AssignTeacherAsync(), () => SelectedAvailableTeacher != null);
        RemoveCommand = new RelayCommand(async () => await RemoveTeacherAsync(), () => SelectedAssignedTeacher != null);
        CloseCommand = new RelayCommand(() => OnClosed?.Invoke());
    }

    public async Task InitializeAsync(int classId)
    {
        _classId = classId;
        await LoadTeachersAsync();
    }

    private async Task LoadTeachersAsync()
    {
        try
        {
            IsLoading = true;
            var allTeachers = await _userService.GetTeachersByRoleAsync();
            var assignedTeachers = await _assignmentService.GetTeachersByClassAsync(_classId);

            var assignedIds = assignedTeachers.Select(t => t.UserId).ToHashSet();
            var available = allTeachers.Where(t => !assignedIds.Contains(t.UserId)).ToList();

            AvailableTeachers = new ObservableCollection<User>(available);
            AssignedTeachers = new ObservableCollection<User>(assignedTeachers);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Lỗi khi tải danh sách giáo viên: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task AssignTeacherAsync()
    {
        if (SelectedAvailableTeacher == null)
            return;

        try
        {
            var success = await _assignmentService.AssignTeacherToClassAsync(SelectedAvailableTeacher.UserId, _classId);
            if (success)
            {
                await LoadTeachersAsync();
                MessageBox.Show("Gán giáo viên thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                ErrorMessage = "Gán giáo viên thất bại!";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Lỗi: {ex.Message}";
        }
    }

    private async Task RemoveTeacherAsync()
    {
        if (SelectedAssignedTeacher == null)
            return;

        var result = MessageBox.Show(
            $"Bạn có chắc chắn muốn xóa giáo viên '{SelectedAssignedTeacher.FullName}'?",
            "Xác nhận xóa",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            try
            {
                var success = await _assignmentService.RemoveTeacherFromClassAsync(SelectedAssignedTeacher.UserId, _classId);
                if (success)
                {
                    await LoadTeachersAsync();
                    MessageBox.Show("Xóa giáo viên thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    ErrorMessage = "Xóa giáo viên thất bại!";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Lỗi: {ex.Message}";
            }
        }
    }

    public event Action? OnClosed;
}

