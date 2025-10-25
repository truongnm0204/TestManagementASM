using System.Windows;
using System.Windows.Input;
using TestManagementASM.Services.Interfaces;
using TestManagementASM.ViewModels;

namespace TestManagementASM.Commands;

public class LoginCommand : ICommand
{
    private readonly LoginViewModel _viewModel;
    private readonly IAuthenticationService _authService;
    private readonly Action _onLoginSuccess;

    public LoginCommand(LoginViewModel viewModel, IAuthenticationService authService, Action onLoginSuccess)
    {
        _viewModel = viewModel;
        _authService = authService;
        _onLoginSuccess = onLoginSuccess;
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object? parameter)
    {
        return !string.IsNullOrWhiteSpace(_viewModel.Username) &&
               !string.IsNullOrWhiteSpace(_viewModel.Password);
    }

    public async void Execute(object? parameter)
    {
        try
        {
            _viewModel.IsLoading = true;
            _viewModel.ErrorMessage = string.Empty;

            var user = await _authService.LoginAsync(_viewModel.Username, _viewModel.Password);

            if (user != null)
            {
                _onLoginSuccess();
            }
            else
            {
                _viewModel.ErrorMessage = "Tên đăng nhập hoặc mật khẩu không đúng!";
            }
        }
        catch (Exception ex)
        {
            _viewModel.ErrorMessage = $"Lỗi: {ex.Message}";
        }
        finally
        {
            _viewModel.IsLoading = false;
        }
    }
}
