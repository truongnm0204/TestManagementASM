using Microsoft.EntityFrameworkCore;
using TestManagementASM.Helpers;
using TestManagementASM.Models;
using TestManagementASM.Services.Interfaces;
using TestManagementASM.Stores;

namespace TestManagementASM.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly TestManagementDbContext _context;
    private readonly AuthStore _authStore;

    public AuthenticationService(TestManagementDbContext context, AuthStore authStore)
    {
        _context = context;
        _authStore = authStore;
    }

    public async Task<User?> LoginAsync(string username, string password)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username == username && u.Status == 1);

        if (user == null)
            return null;

        if (!PasswordHasher.VerifyPassword(password, user.PasswordHash))
            return null;

        _authStore.CurrentUser = user;
        return user;
    }

    public void Logout()
    {
        _authStore.Logout();
    }

    public User? GetCurrentUser()
    {
        return _authStore.CurrentUser;
    }
}
