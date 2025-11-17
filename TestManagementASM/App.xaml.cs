using System.Windows;
using TestManagementASM.Models;
using TestManagementASM.Services;
using TestManagementASM.Services.Interfaces;
using TestManagementASM.Stores;
using TestManagementASM.ViewModels;
using TestManagementASM.ViewModels.Base;
using TestManagementASM.ViewModels.Student;
using TestManagementASM.ViewModels.Teacher;
using TestManagementASM.Views;

namespace TestManagementASM;

public partial class App : Application
{
    private readonly NavigationStore _navigationStore;
    private readonly AuthStore _authStore;
    private readonly SelectedTestStore _selectedTestStore;
    private readonly SelectedQuestionStore _selectedQuestionStore;
    private readonly SelectedClassStore _selectedClassStore;
    private readonly SelectedStudentStore _selectedStudentStore;

    public App()
    {
        _navigationStore = new NavigationStore();
        _authStore = new AuthStore();
        _selectedTestStore = new SelectedTestStore();
        _selectedQuestionStore = new SelectedQuestionStore();
        _selectedClassStore = new SelectedClassStore();
        _selectedStudentStore = new SelectedStudentStore();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Create services - each with its own DbContext to avoid concurrency issues
        IAuthenticationService authService = new AuthenticationService(new TestManagementDbContext(), _authStore);
        ISubjectService subjectService = new SubjectService(new TestManagementDbContext(), _authStore);
        IUserService userService = new UserService(new TestManagementDbContext());
        ITestService testService = new TestService(new TestManagementDbContext());
        IQuestionService questionService = new QuestionService(new TestManagementDbContext());
        ITestAttemptService testAttemptService = new TestAttemptService(new TestManagementDbContext());
        IClassService classService = new ClassService(new TestManagementDbContext());
        IEnrollmentService enrollmentService = new EnrollmentService(new TestManagementDbContext());

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
            try
            {
                var mainViewModel = new MainViewModel(_navigationStore, _authStore, navigationService, authService);

                var mainWindow = new MainWindow
                {
                    DataContext = mainViewModel
                };

                // Set MainWindow as the main window and change shutdown mode
                Current.MainWindow = mainWindow;
                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

                mainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở MainWindow: {ex.Message}\n\nStackTrace:\n{ex.StackTrace}\n\nInnerException: {ex.InnerException?.Message}",
                    "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
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
        // Create separate DbContext instances for each service to avoid concurrency issues
        // Each service gets its own DbContext to prevent threading conflicts
        IAuthenticationService authService = new AuthenticationService(new TestManagementDbContext(), _authStore);
        ISubjectService subjectService = new SubjectService(new TestManagementDbContext(), _authStore);
        IUserService userService = new UserService(new TestManagementDbContext());
        ITestService testService = new TestService(new TestManagementDbContext());
        IQuestionService questionService = new QuestionService(new TestManagementDbContext());
        ITestAttemptService testAttemptService = new TestAttemptService(new TestManagementDbContext());
        IClassService classService = new ClassService(new TestManagementDbContext());
        IEnrollmentService enrollmentService = new EnrollmentService(new TestManagementDbContext());
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
        // Student ViewModels
        else if (viewModelType == typeof(StudentTestListViewModel))
        {
            return new StudentTestListViewModel(testService, _authStore, _selectedTestStore, navigationService);
        }
        else if (viewModelType == typeof(TestDetailViewModel))
        {
            return new TestDetailViewModel(testService, testAttemptService, _authStore, _selectedTestStore, navigationService);
        }
        else if (viewModelType == typeof(TakeTestViewModel))
        {
            return new TakeTestViewModel(testService, testAttemptService, _authStore, _selectedTestStore, navigationService);
        }
        // Teacher ViewModels
        else if (viewModelType == typeof(TeacherClassListViewModel))
        {
            return new TeacherClassListViewModel(classService, _authStore, _selectedClassStore, navigationService);
        }
        else if (viewModelType == typeof(ClassDetailViewModel))
        {
            return new ClassDetailViewModel(classService, testAttemptService, _selectedClassStore, _selectedStudentStore, navigationService);
        }
        else if (viewModelType == typeof(StudentDetailViewModel))
        {
            return new StudentDetailViewModel(testAttemptService, _selectedStudentStore, navigationService);
        }
        else if (viewModelType == typeof(TeacherTestListViewModel))
        {
            return new TeacherTestListViewModel(testService, _authStore, _selectedTestStore, navigationService);
        }
        else if (viewModelType == typeof(TestFormViewModel))
        {
            return new TestFormViewModel(testService, classService, questionService, subjectService, _authStore, _selectedTestStore, navigationService);
        }
        else if (viewModelType == typeof(QuestionListViewModel))
        {
            return new QuestionListViewModel(questionService, subjectService, _authStore, _selectedQuestionStore, navigationService);
        }
        else if (viewModelType == typeof(QuestionFormViewModel))
        {
            return new QuestionFormViewModel(questionService, subjectService, _authStore, _selectedQuestionStore, navigationService);
        }
        else
        {
            throw new ArgumentException($"Unknown ViewModel type: {viewModelType.Name}");
        }
    }

    protected override void OnExit(ExitEventArgs e)
    {
        // DbContext instances are now created per-service and will be garbage collected
        base.OnExit(e);
    }
}
