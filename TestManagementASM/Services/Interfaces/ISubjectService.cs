using TestManagementASM.Models;

namespace TestManagementASM.Services.Interfaces;

public interface ISubjectService
{
    Task<List<Subject>> GetAllSubjectsAsync();
    Task<Subject?> GetSubjectByIdAsync(int id);
    Task<bool> AddSubjectAsync(Subject subject);
    Task<bool> UpdateSubjectAsync(Subject subject);
    Task<bool> DeleteSubjectAsync(int id);
    Task<bool> HasClassesAsync(int subjectId);
    Task<bool> IsSubjectCodeUniqueAsync(string subjectCode, int? excludeSubjectId = null);
}
