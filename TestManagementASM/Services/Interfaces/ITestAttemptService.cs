using TestManagementASM.Models;

namespace TestManagementASM.Services.Interfaces;

public interface ITestAttemptService
{
    Task<TestAttempt> CreateAttemptAsync(int studentId, int testId);
    Task<bool> SaveStudentAnswerAsync(int attemptId, int questionId, int answerId);
    Task<bool> CompleteAttemptAsync(int attemptId);
    Task<List<TestAttempt>> GetStudentAttemptsAsync(int studentId, int? classId = null);
    Task<List<TestAttempt>> GetClassTestScoresAsync(int classId);
    Task<TestAttempt?> GetAttemptByIdAsync(int attemptId);
}

