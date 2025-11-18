using Microsoft.EntityFrameworkCore;
using TestManagementASM.Helpers;
using TestManagementASM.Models;
using TestManagementASM.Services.Interfaces;

namespace TestManagementASM.Services;

public class UserService : IUserService
{
    private readonly TestManagementDbContext _context;

    public UserService(TestManagementDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users
            .Include(u => u.Role)
            .OrderByDescending(u => u.UserId)
            .ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.UserId == id);
    }

    public async Task<bool> AddUserAsync(User user)
    {
        try
        {
            // Password should already be hashed by the caller (UserFormView.xaml.cs)
            // Do NOT hash again to avoid double hashing
            user.CreatedAt = DateTime.Now;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        try
        {
            // Attach the user to the context if it's not already tracked
            var existingUser = await _context.Users.FindAsync(user.UserId);
            if (existingUser != null)
            {
                // Update properties
                existingUser.Username = user.Username;
                existingUser.FullName = user.FullName;
                existingUser.Email = user.Email;
                existingUser.RoleId = user.RoleId;
                existingUser.Status = user.Status;

                _context.Users.Update(existingUser);
            }
            else
            {
                _context.Users.Update(user);
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"UpdateUserAsync Error: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<User>> GetTeachersByRoleAsync()
    {
        return await _context.Users
            .Where(u => u.RoleId == 2) // RoleId 2 = Teacher
            .Include(u => u.Role)
            .OrderBy(u => u.FullName)
            .ToListAsync();
    }

    public async Task<bool> IsUsernameUniqueAsync(string username, int? excludeUserId = null)
    {
        var query = _context.Users.Where(u => u.Username == username);

        if (excludeUserId.HasValue)
            query = query.Where(u => u.UserId != excludeUserId.Value);

        return !await query.AnyAsync();
    }
}
