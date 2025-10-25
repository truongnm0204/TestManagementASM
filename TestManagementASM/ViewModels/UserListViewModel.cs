using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TestManagementASM.Models;
using TestManagementASM.Services.Interfaces;
using TestManagementASM.ViewModels.Base;

namespace TestManagementASM.ViewModels;

public class UserListViewModel : ViewModelBase
{
    private readonly IUserService _userService;
    private ObservableCollection<User> _users = new();
    private User? _selectedUser;
    private string _searchText = string.Empty;
    private bool _isLoading;

    public ObservableCollection<User> Users
    {
        get => _users;
        set => SetProperty(ref _users, value);
    }

    public User? SelectedUser
    {
        get => _selectedUser;
        set => SetProperty(ref _selectedUser, value);
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            SetProperty(ref _searchText, value);
            _ = LoadUsersAsync();
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public ICommand LoadDataCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand ViewDetailsCommand { get; }

    public UserListViewModel(IUserService userService)
    {
        _userService = userService;

        LoadDataCommand = new RelayCommand(async () => await LoadUsersAsync());
        RefreshCommand = new RelayCommand(async () => await LoadUsersAsync());
        ViewDetailsCommand = new RelayCommand(ViewUserDetails, () => SelectedUser != null);

        _ = LoadUsersAsync();
    }

    private async Task LoadUsersAsync()
    {
        try
        {
            IsLoading = true;
            var users = await _userService.GetAllUsersAsync();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                users = users.Where(u =>
                    (u.Username?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (u.FullName?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (u.Email?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false)
                ).ToList();
            }

            Users = new ObservableCollection<User>(users);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi tải danh sách người dùng: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void ViewUserDetails()
    {
        if (SelectedUser != null)
        {
            var details = $"ID: {SelectedUser.UserId}\n" +
                         $"Username: {SelectedUser.Username}\n" +
                         $"Họ tên: {SelectedUser.FullName ?? "N/A"}\n" +
                         $"Email: {SelectedUser.Email ?? "N/A"}\n" +
                         $"Vai trò: {SelectedUser.Role?.RoleName ?? "N/A"}\n" +
                         $"Trạng thái: {(SelectedUser.Status == 1 ? "Hoạt động" : "Không hoạt động")}\n" +
                         $"Ngày tạo: {SelectedUser.CreatedAt?.ToString("dd/MM/yyyy HH:mm") ?? "N/A"}";

            MessageBox.Show(details, "Chi tiết người dùng", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
