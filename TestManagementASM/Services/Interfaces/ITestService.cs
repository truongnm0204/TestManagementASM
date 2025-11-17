using TestManagementASM.Models;

namespace TestManagementASM.Services.Interfaces;

public interface ITestService
{
    // Student operations
    Task<List<Test>> GetAvailableTestsForStudentAsync(int studentId);
    Task<Test?> GetTestByIdAsync(int testId);
    Task<List<Question>> GetTestQuestionsAsync(int testId);
    Task<int> GetTestQuestionCountAsync(int testId);
    
    // Teacher operations
    Task<List<Test>> GetTestsByTeacherAsync(int teacherId);
    Task<bool> CreateTestAsync(Test test, List<TestQuestion> testQuestions);
    Task<bool> UpdateTestAsync(Test test);
    Task<bool> DeleteTestAsync(int testId);
    Task<bool> ToggleTestActiveStatusAsync(int testId);
}

