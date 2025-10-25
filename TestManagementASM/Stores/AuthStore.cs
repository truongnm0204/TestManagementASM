using TestManagementASM.Models;

namespace TestManagementASM.Stores;

public class AuthStore
{
    private User? _currentUser;

    public User? CurrentUser
    {
        get => _currentUser;
        set
        {
            _currentUser = value;
            OnCurrentUserChanged();
        }
    }

    public bool IsLoggedIn => CurrentUser != null;

    public event Action? CurrentUserChanged;

    private void OnCurrentUserChanged()
    {
        CurrentUserChanged?.Invoke();
    }

    public void Logout()
    {
        CurrentUser = null;
    }
}
