using TestManagementASM.Models;

namespace TestManagementASM.Services.Interfaces;

public interface IClassService
{
    // Existing methods
    Task<List<Class>> GetTeacherClassesAsync(int teacherId);
    Task<Class?> GetClassByIdAsync(int classId);
    Task<List<User>> GetStudentsByClassAsync(int classId);

    // New CRUD methods
    Task<List<Class>> GetAllClassesAsync();
    Task<bool> AddClassAsync(Class @class);
    Task<bool> UpdateClassAsync(Class @class);
    Task<bool> DeleteClassAsync(int classId);
}

