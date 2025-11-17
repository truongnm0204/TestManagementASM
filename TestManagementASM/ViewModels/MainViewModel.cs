using System;
using System.Windows;
using System.Windows.Input;
using TestManagementASM.Commands;
using TestManagementASM.Services.Interfaces;
using TestManagementASM.Stores;
using TestManagementASM.ViewModels.Base;
using TestManagementASM.ViewModels.Student;
using TestManagementASM.ViewModels.Teacher;

namespace TestManagementASM.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly NavigationStore _navigationStore;
    private readonly AuthStore _authStore;
    private readonly INavigationService _navigationService;
    private string _currentUserName = string.Empty;
    private string _currentUserRole = string.Empty;

    public ViewModelBase? CurrentViewModel => _navigationStore.CurrentViewModel;

    public string CurrentUserName
    {
        get => _currentUserName;
        set => SetProperty(ref _currentUserName, value);
    }

    public string CurrentUserRole
    {
        get => _currentUserRole;
        set => SetProperty(ref _currentUserRole, value);
    }

    // Role-based visibility
    public int? CurrentUserRoleId => _authStore.CurrentUser?.RoleId;
    public bool IsAdmin => _authStore.CurrentUser?.RoleId == 1;
    public bool IsTeacher => _authStore.CurrentUser?.RoleId == 2;
    public bool IsStudent => _authStore.CurrentUser?.RoleId == 3;

    public ICommand NavigateToSubjectsCommand { get; }
    public ICommand NavigateToUsersCommand { get; }
    public ICommand NavigateToStudentTestsCommand { get; }
    public ICommand NavigateToTeacherClassesCommand { get; }
    public ICommand NavigateToTeacherTestsCommand { get; }
    public ICommand NavigateToQuestionsCommand { get; }
    public ICommand LogoutCommand { get; }

    public MainViewModel(
        NavigationStore navigationStore,
        AuthStore authStore,
        INavigationService navigationService,
        IAuthenticationService authService)
    {
        _navigationStore = navigationStore;
        _authStore = authStore;
        _navigationService = navigationService;

        _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

        NavigateToSubjectsCommand = new NavigateCommand<SubjectListViewModel>(navigationService);
        NavigateToUsersCommand = new NavigateCommand<UserListViewModel>(navigationService);
        NavigateToStudentTestsCommand = new NavigateCommand<StudentTestListViewModel>(navigationService);
        NavigateToTeacherClassesCommand = new NavigateCommand<TeacherClassListViewModel>(navigationService);
        NavigateToTeacherTestsCommand = new NavigateCommand<TeacherTestListViewModel>(navigationService);
        NavigateToQuestionsCommand = new NavigateCommand<QuestionListViewModel>(navigationService);
        LogoutCommand = new RelayCommand(() => Logout(authService));

        UpdateCurrentUserInfo();

        // Navigate based on role
        try
        {
            if (IsStudent)
            {
                _navigationService.NavigateTo<StudentTestListViewModel>();
            }
            else if (IsTeacher)
            {
                _navigationService.NavigateTo<TeacherClassListViewModel>();
            }
            else
            {
                _navigationService.NavigateTo<UserListViewModel>();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi navigate: {ex.Message}\n\nStackTrace: {ex.StackTrace}\n\nInnerException: {ex.InnerException?.Message}",
                "Lỗi Navigation", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void OnCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }

    private void UpdateCurrentUserInfo()
    {
        var currentUser = _authStore.CurrentUser;
        if (currentUser != null)
        {
            CurrentUserName = currentUser.FullName ?? currentUser.Username;
            CurrentUserRole = currentUser.Role?.RoleName ?? "Unknown";
        }

        // Notify role-based properties
        OnPropertyChanged(nameof(CurrentUserRoleId));
        OnPropertyChanged(nameof(IsAdmin));
        OnPropertyChanged(nameof(IsTeacher));
        OnPropertyChanged(nameof(IsStudent));
    }

    private void Logout(IAuthenticationService authService)
    {
        var result = MessageBox.Show(
            "Bạn có chắc chắn muốn đăng xuất?",
            "Xác nhận đăng xuất",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            authService.Logout();
            Application.Current.Shutdown();
        }
    }
}
