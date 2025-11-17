using TestManagementASM.Models;

namespace TestManagementASM.Services.Interfaces;

public interface IUserService
{
    Task<List<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(int id);
    Task<bool> AddUserAsync(User user);
    Task<bool> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(int id);
    Task<List<User>> GetTeachersByRoleAsync();
    Task<bool> IsUsernameUniqueAsync(string username, int? excludeUserId = null);
}
