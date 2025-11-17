using TestManagementASM.Models;

namespace TestManagementASM.Services.Interfaces;

public interface ITeachingAssignmentService
{
    Task<List<User>> GetTeachersByClassAsync(int classId);
    Task<List<Class>> GetClassesByTeacherAsync(int teacherId);
    Task<bool> AssignTeacherToClassAsync(int teacherId, int classId);
    Task<bool> RemoveTeacherFromClassAsync(int teacherId, int classId);
    Task<bool> IsTeacherAssignedAsync(int teacherId, int classId);
}

