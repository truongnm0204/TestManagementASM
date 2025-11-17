using Microsoft.EntityFrameworkCore;
using TestManagementASM.Models;
using TestManagementASM.Services.Interfaces;

namespace TestManagementASM.Services;

public class TestAttemptService : ITestAttemptService
{
    private readonly TestManagementDbContext _context;

    public TestAttemptService(TestManagementDbContext context)
    {
        _context = context;
    }

    public async Task<TestAttempt> CreateAttemptAsync(int studentId, int testId)
    {
        var attempt = new TestAttempt
        {
            StudentId = studentId,
            TestId = testId,
            StartTime = DateTime.Now,
            AttemptStatus = "InProgress",
            Score = 0
        };

        _context.TestAttempts.Add(attempt);
        await _context.SaveChangesAsync();

        return attempt;
    }

    public async Task<bool> SaveStudentAnswerAsync(int attemptId, int questionId, int answerId)
    {
        try
        {
            // Check if answer already exists for this question in this attempt
            var existingAnswer = await _context.StudentAnswers
                .FirstOrDefaultAsync(sa => sa.AttemptId == attemptId && sa.QuestionId == questionId);

            if (existingAnswer != null)
            {
                // Update existing answer
                existingAnswer.ChosenAnswerId = answerId;
                _context.StudentAnswers.Update(existingAnswer);
            }
            else
            {
                // Create new answer
                var studentAnswer = new StudentAnswer
                {
                    AttemptId = attemptId,
                    QuestionId = questionId,
                    ChosenAnswerId = answerId
                };
                _context.StudentAnswers.Add(studentAnswer);
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> CompleteAttemptAsync(int attemptId)
    {
        try
        {
            var attempt = await _context.TestAttempts
                .Include(a => a.StudentAnswers)
                    .ThenInclude(sa => sa.ChosenAnswer)
                .Include(a => a.Test)
                    .ThenInclude(t => t.TestQuestions)
                .FirstOrDefaultAsync(a => a.AttemptId == attemptId);

            if (attempt == null) return false;

            // Calculate score
            int totalPoints = attempt.Test.TestQuestions.Sum(tq => tq.Points);
            int earnedPoints = 0;

            foreach (var studentAnswer in attempt.StudentAnswers)
            {
                if (studentAnswer.ChosenAnswer.IsCorrect)
                {
                    var testQuestion = attempt.Test.TestQuestions
                        .FirstOrDefault(tq => tq.QuestionId == studentAnswer.QuestionId);
                    if (testQuestion != null)
                    {
                        earnedPoints += testQuestion.Points;
                    }
                }
            }

            // Calculate percentage score (0-100)
            attempt.Score = totalPoints > 0 ? (int)((double)earnedPoints / totalPoints * 100) : 0;
            attempt.EndTime = DateTime.Now;
            attempt.AttemptStatus = "Completed";

            _context.TestAttempts.Update(attempt);
            await _context.SaveChangesAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<TestAttempt>> GetStudentAttemptsAsync(int studentId, int? classId = null)
    {
        var query = _context.TestAttempts
            .Include(a => a.Test)
                .ThenInclude(t => t.Class)
                    .ThenInclude(c => c.Subject)
            .Where(a => a.StudentId == studentId);

        if (classId.HasValue)
        {
            query = query.Where(a => a.Test.ClassId == classId.Value);
        }

        return await query
            .OrderByDescending(a => a.StartTime)
            .ToListAsync();
    }

    public async Task<List<TestAttempt>> GetClassTestScoresAsync(int classId)
    {
        return await _context.TestAttempts
            .Include(a => a.Student)
            .Include(a => a.Test)
            .Where(a => a.Test.ClassId == classId)
            .OrderBy(a => a.Student.FullName)
            .ThenByDescending(a => a.StartTime)
            .ToListAsync();
    }

    public async Task<TestAttempt?> GetAttemptByIdAsync(int attemptId)
    {
        return await _context.TestAttempts
            .Include(a => a.Student)
            .Include(a => a.Test)
                .ThenInclude(t => t.Class)
            .Include(a => a.StudentAnswers)
                .ThenInclude(sa => sa.ChosenAnswer)
            .FirstOrDefaultAsync(a => a.AttemptId == attemptId);
    }
}

