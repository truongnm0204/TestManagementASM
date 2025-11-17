using TestManagementASM.Models;

namespace TestManagementASM.Services.Interfaces;

public interface IClassService
{
    Task<List<Class>> GetTeacherClassesAsync(int teacherId);
    Task<Class?> GetClassByIdAsync(int classId);
    Task<List<User>> GetStudentsByClassAsync(int classId);
}

