using TestManagementASM.Models;

namespace TestManagementASM.Services.Interfaces;

public interface IQuestionService
{
    Task<List<Question>> GetQuestionsByTeacherAsync(int teacherId);
    Task<List<Question>> GetQuestionsBySubjectAsync(int subjectId);
    Task<Question?> GetQuestionByIdAsync(int questionId);
    Task<bool> CreateQuestionAsync(Question question, List<Answer> answers);
    Task<bool> UpdateQuestionAsync(Question question, List<Answer> answers);
    Task<bool> DeleteQuestionAsync(int questionId);
}

