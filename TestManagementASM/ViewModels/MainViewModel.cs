using System.Windows;
using System.Windows.Input;
using TestManagementASM.Commands;
using TestManagementASM.Services.Interfaces;
using TestManagementASM.Stores;
using TestManagementASM.ViewModels.Base;

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

    public ICommand NavigateToSubjectsCommand { get; }
    public ICommand NavigateToUsersCommand { get; }
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
        LogoutCommand = new RelayCommand(() => Logout(authService));

        UpdateCurrentUserInfo();

        // Navigate to Users by default
        _navigationService.NavigateTo<UserListViewModel>();
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
