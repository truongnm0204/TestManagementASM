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

    public async Task<bool> SaveStudentAnswersAsync(int attemptId, int questionId, List<int> answerIds)
    {
        try
        {
            // Remove all existing answers for this question in this attempt
            var existingAnswers = await _context.StudentAnswers
                .Where(sa => sa.AttemptId == attemptId && sa.QuestionId == questionId)
                .ToListAsync();

            if (existingAnswers.Any())
            {
                _context.StudentAnswers.RemoveRange(existingAnswers);
            }

            // Add new answers
            foreach (var answerId in answerIds)
            {
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
                .Include(a => a.StudentAnswers)
                    .ThenInclude(sa => sa.Question)
                        .ThenInclude(q => q.Answers)
                .Include(a => a.Test)
                    .ThenInclude(t => t.TestQuestions)
                        .ThenInclude(tq => tq.Question)
                .FirstOrDefaultAsync(a => a.AttemptId == attemptId);

            if (attempt == null) return false;

            // Calculate score
            int totalPoints = attempt.Test.TestQuestions.Sum(tq => tq.Points);
            double earnedPoints = 0;

            // Group student answers by question
            var answersByQuestion = attempt.StudentAnswers
                .GroupBy(sa => sa.QuestionId)
                .ToList();

            foreach (var questionGroup in answersByQuestion)
            {
                var questionId = questionGroup.Key;
                var testQuestion = attempt.Test.TestQuestions
                    .FirstOrDefault(tq => tq.QuestionId == questionId);

                if (testQuestion == null) continue;

                var question = testQuestion.Question;
                var studentAnswerIds = questionGroup.Select(sa => sa.ChosenAnswerId).ToHashSet();
                var correctAnswerIds = question.Answers
                    .Where(a => a.IsCorrect)
                    .Select(a => a.AnswerId)
                    .ToHashSet();

                // Check question type
                if (question.QuestionType == "SINGLE")
                {
                    // Single choice: Award full points if correct answer is selected
                    if (studentAnswerIds.Count == 1 && correctAnswerIds.Contains(studentAnswerIds.First()))
                    {
                        earnedPoints += testQuestion.Points;
                    }
                }
                else if (question.QuestionType == "MULTIPLE")
                {
                    // Multiple choice: All-or-nothing scoring
                    // Student must select ALL correct answers and NO incorrect answers
                    if (studentAnswerIds.SetEquals(correctAnswerIds))
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

