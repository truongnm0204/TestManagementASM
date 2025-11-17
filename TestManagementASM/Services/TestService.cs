using Microsoft.EntityFrameworkCore;
using TestManagementASM.Models;
using TestManagementASM.Services.Interfaces;

namespace TestManagementASM.Services;

public class TestService : ITestService
{
    private readonly TestManagementDbContext _context;

    public TestService(TestManagementDbContext context)
    {
        _context = context;
    }

    public async Task<List<Test>> GetAvailableTestsForStudentAsync(int studentId)
    {
        var now = DateTime.Now;
        
        return await _context.Tests
            .Include(t => t.Class)
                .ThenInclude(c => c.Subject)
            .Include(t => t.TestQuestions)
            .Where(t => t.IsActive &&
                       t.Class.Enrollments.Any(e => e.StudentId == studentId) &&
                       (!t.AvailableFrom.HasValue || t.AvailableFrom <= now) &&
                       (!t.AvailableTo.HasValue || t.AvailableTo >= now))
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<Test?> GetTestByIdAsync(int testId)
    {
        return await _context.Tests
            .Include(t => t.Class)
                .ThenInclude(c => c.Subject)
            .Include(t => t.TestQuestions)
                .ThenInclude(tq => tq.Question)
                    .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(t => t.TestId == testId);
    }

    public async Task<List<Question>> GetTestQuestionsAsync(int testId)
    {
        return await _context.TestQuestions
            .Where(tq => tq.TestId == testId)
            .Include(tq => tq.Question)
                .ThenInclude(q => q.Answers)
            .Select(tq => tq.Question)
            .ToListAsync();
    }

    public async Task<int> GetTestQuestionCountAsync(int testId)
    {
        return await _context.TestQuestions
            .Where(tq => tq.TestId == testId)
            .CountAsync();
    }

    public async Task<List<Test>> GetTestsByTeacherAsync(int teacherId)
    {
        return await _context.Tests
            .Include(t => t.Class)
                .ThenInclude(c => c.Subject)
            .Include(t => t.TestQuestions)
            .Where(t => t.CreatedByTeacherId == teacherId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> CreateTestAsync(Test test, List<TestQuestion> testQuestions)
    {
        try
        {
            test.CreatedAt = DateTime.Now;
            _context.Tests.Add(test);
            await _context.SaveChangesAsync();

            // Add test questions
            foreach (var tq in testQuestions)
            {
                tq.TestId = test.TestId;
                _context.TestQuestions.Add(tq);
            }
            
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateTestAsync(Test test)
    {
        try
        {
            _context.Tests.Update(test);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteTestAsync(int testId)
    {
        try
        {
            var test = await _context.Tests.FindAsync(testId);
            if (test == null) return false;

            _context.Tests.Remove(test);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> ToggleTestActiveStatusAsync(int testId)
    {
        try
        {
            var test = await _context.Tests.FindAsync(testId);
            if (test == null) return false;

            test.IsActive = !test.IsActive;
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}

