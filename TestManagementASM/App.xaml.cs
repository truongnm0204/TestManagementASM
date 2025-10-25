using System.Windows;
using TestManagementASM.Models;
using TestManagementASM.Services;
using TestManagementASM.Services.Interfaces;
using TestManagementASM.Stores;
using TestManagementASM.ViewModels;
using TestManagementASM.ViewModels.Base;
using TestManagementASM.Views;

namespace TestManagementASM;

public partial class App : Application
{
    private readonly NavigationStore _navigationStore;
    private readonly AuthStore _authStore;
    private readonly TestManagementDbContext _dbContext;

    public App()
    {
        _navigationStore = new NavigationStore();
        _authStore = new AuthStore();
        _dbContext = new TestManagementDbContext();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Create services
        IAuthenticationService authService = new AuthenticationService(_dbContext, _authStore);
        ISubjectService subjectService = new SubjectService(_dbContext, _authStore);
        IUserService userService = new UserService(_dbContext);

        // Create navigation service with ViewModel factory
        INavigationService navigationService = new NavigationService(
            _navigationStore,
            CreateViewModel
        );

        // Create and show login window
        var loginViewModel = new LoginViewModel(authService, () => OnLoginSuccess(navigationService, authService));
        var loginWindow = new LoginWindow
        {
            DataContext = loginViewModel
        };

        loginWindow.ShowDialog();

        // If login successful, show main window
        if (_authStore.IsLoggedIn)
        {
            var mainViewModel = new MainViewModel(_navigationStore, _authStore, navigationService, authService);
            var mainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };
            mainWindow.Show();
        }
        else
        {
            // User closed login window without logging in
            Shutdown();
        }
    }

    private void OnLoginSuccess(INavigationService navigationService, IAuthenticationService authService)
    {
        // Close all windows (login window) with DialogResult
        foreach (Window window in Current.Windows)
        {
            if (window is LoginWindow loginWindow)
            {
                loginWindow.DialogResult = true;
                loginWindow.Close();
            }
        }
    }

    private ViewModelBase CreateViewModel(Type viewModelType)
    {
        // Services needed for ViewModels
        IAuthenticationService authService = new AuthenticationService(_dbContext, _authStore);
        ISubjectService subjectService = new SubjectService(_dbContext, _authStore);
        IUserService userService = new UserService(_dbContext);
        INavigationService navigationService = new NavigationService(_navigationStore, CreateViewModel);

        // Factory method to create ViewModels
        if (viewModelType == typeof(SubjectListViewModel))
        {
            return new SubjectListViewModel(subjectService);
        }
        else if (viewModelType == typeof(UserListViewModel))
        {
            return new UserListViewModel(userService);
        }
        else if (viewModelType == typeof(MainViewModel))
        {
            return new MainViewModel(_navigationStore, _authStore, navigationService, authService);
        }

        throw new ArgumentException($"Unknown ViewModel type: {viewModelType.Name}");
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _dbContext?.Dispose();
        base.OnExit(e);
    }
}
