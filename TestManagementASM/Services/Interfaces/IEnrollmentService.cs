using TestManagementASM.Models;

namespace TestManagementASM.Services.Interfaces;

public interface IEnrollmentService
{
    Task<List<Class>> GetStudentClassesAsync(int studentId);
    Task<bool> IsStudentEnrolledAsync(int studentId, int classId);
}

