using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TestManagementASM.Commands;
using TestManagementASM.Models;
using TestManagementASM.Services.Interfaces;
using TestManagementASM.ViewModels.Base;

namespace TestManagementASM.ViewModels;

public class UserFormViewModel : ViewModelBase
{
    private readonly IUserService _userService;
    private User _user = new();
    private ObservableCollection<Role> _roles = new();
    private bool _isEditMode;
    private bool _isLoading;
    private string _errorMessage = string.Empty;

    public User User
    {
        get => _user;
        set => SetProperty(ref _user, value);
    }

    public ObservableCollection<Role> Roles
    {
        get => _roles;
        set => SetProperty(ref _roles, value);
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

    public UserFormViewModel(IUserService userService)
    {
        _userService = userService;
        SaveCommand = new RelayCommand(async () => await SaveAsync());
        CancelCommand = new RelayCommand(() => OnClosed?.Invoke());

        LoadRoles();
    }

    private void LoadRoles()
    {
        // Hardcoded roles - in production, fetch from database
        Roles = new ObservableCollection<Role>
        {
            new() { RoleId = 1, RoleName = "Admin" },
            new() { RoleId = 2, RoleName = "Teacher" },
            new() { RoleId = 3, RoleName = "Student" }
        };
    }

    public void InitializeForCreate()
    {
        IsEditMode = false;
        User = new User { Status = 1 };
        ErrorMessage = string.Empty;
    }

    public void InitializeForEdit(User user)
    {
        IsEditMode = true;
        User = new User
        {
            UserId = user.UserId,
            Username = user.Username,
            FullName = user.FullName,
            Email = user.Email,
            RoleId = user.RoleId,
            Status = user.Status
        };
        ErrorMessage = string.Empty;
    }

    private async Task SaveAsync()
    {
        try
        {
            ErrorMessage = string.Empty;

            // Validation
            if (string.IsNullOrWhiteSpace(User.Username))
            {
                ErrorMessage = "Username không được để trống!";
                return;
            }

            if (IsEditMode)
            {
                await _userService.UpdateUserAsync(User);
                MessageBox.Show("Cập nhật người dùng thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(User.PasswordHash))
                {
                    ErrorMessage = "Mật khẩu không được để trống!";
                    return;
                }

                var isUnique = await _userService.IsUsernameUniqueAsync(User.Username);
                if (!isUnique)
                {
                    ErrorMessage = "Username đã tồn tại!";
                    return;
                }

                await _userService.AddUserAsync(User);
                MessageBox.Show("Tạo người dùng thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
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

