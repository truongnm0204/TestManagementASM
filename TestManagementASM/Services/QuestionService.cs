using Microsoft.EntityFrameworkCore;
using TestManagementASM.Models;
using TestManagementASM.Services.Interfaces;

namespace TestManagementASM.Services;

public class QuestionService : IQuestionService
{
    private readonly TestManagementDbContext _context;

    public QuestionService(TestManagementDbContext context)
    {
        _context = context;
    }

    public async Task<List<Question>> GetQuestionsByTeacherAsync(int teacherId)
    {
        return await _context.Questions
            .Include(q => q.Subject)
            .Include(q => q.Answers)
            .Where(q => q.CreatedByTeacherId == teacherId)
            .OrderByDescending(q => q.QuestionId)
            .ToListAsync();
    }

    public async Task<List<Question>> GetQuestionsBySubjectAsync(int subjectId)
    {
        return await _context.Questions
            .Include(q => q.Subject)
            .Include(q => q.Answers)
            .Where(q => q.SubjectId == subjectId)
            .ToListAsync();
    }

    public async Task<Question?> GetQuestionByIdAsync(int questionId)
    {
        return await _context.Questions
            .Include(q => q.Subject)
            .Include(q => q.Answers)
            .FirstOrDefaultAsync(q => q.QuestionId == questionId);
    }

    public async Task<bool> CreateQuestionAsync(Question question, List<Answer> answers)
    {
        try
        {
            // Validate at least one correct answer
            if (!answers.Any(a => a.IsCorrect))
            {
                return false;
            }

            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            // Add answers
            foreach (var answer in answers)
            {
                answer.QuestionId = question.QuestionId;
                _context.Answers.Add(answer);
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateQuestionAsync(Question question, List<Answer> answers)
    {
        try
        {
            // Validate at least one correct answer
            if (!answers.Any(a => a.IsCorrect))
            {
                return false;
            }

            // Update question
            _context.Questions.Update(question);

            // Remove old answers
            var oldAnswers = await _context.Answers
                .Where(a => a.QuestionId == question.QuestionId)
                .ToListAsync();
            _context.Answers.RemoveRange(oldAnswers);

            // Add new answers
            foreach (var answer in answers)
            {
                answer.QuestionId = question.QuestionId;
                answer.AnswerId = 0; // Reset ID for new insert
                _context.Answers.Add(answer);
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteQuestionAsync(int questionId)
    {
        try
        {
            var question = await _context.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.QuestionId == questionId);
                
            if (question == null) return false;

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}

