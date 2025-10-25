using TestManagementASM.Models;

namespace TestManagementASM.Services.Interfaces;

public interface IAuthenticationService
{
    Task<User?> LoginAsync(string username, string password);
    void Logout();
    User? GetCurrentUser();
}
