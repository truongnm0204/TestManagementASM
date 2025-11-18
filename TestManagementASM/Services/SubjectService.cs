using Microsoft.EntityFrameworkCore;
using TestManagementASM.Models;
using TestManagementASM.Services.Interfaces;
using TestManagementASM.Stores;

namespace TestManagementASM.Services;

public class SubjectService : ISubjectService
{
    private readonly TestManagementDbContext _context;
    private readonly AuthStore _authStore;

    public SubjectService(TestManagementDbContext context, AuthStore authStore)
    {
        _context = context;
        _authStore = authStore;
    }

    public async Task<List<Subject>> GetAllSubjectsAsync()
    {
        return await _context.Subjects
            .Include(s => s.CreatedByUser)
            .OrderByDescending(s => s.SubjectId)
            .ToListAsync();
    }

    public async Task<Subject?> GetSubjectByIdAsync(int id)
    {
        return await _context.Subjects
            .Include(s => s.CreatedByUser)
            .FirstOrDefaultAsync(s => s.SubjectId == id);
    }

    public async Task<bool> AddSubjectAsync(Subject subject)
    {
        try
        {
            subject.CreatedByUserId = _authStore.CurrentUser?.UserId;
            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateSubjectAsync(Subject subject)
    {
        try
        {
            // Attach the subject to the context if it's not already tracked
            var existingSubject = await _context.Subjects.FindAsync(subject.SubjectId);
            if (existingSubject != null)
            {
                // Update properties
                existingSubject.SubjectCode = subject.SubjectCode;
                existingSubject.SubjectName = subject.SubjectName;
                existingSubject.Status = subject.Status;

                _context.Subjects.Update(existingSubject);
            }
            else
            {
                _context.Subjects.Update(subject);
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"UpdateSubjectAsync Error: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteSubjectAsync(int id)
    {
        try
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
                return false;

            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> HasClassesAsync(int subjectId)
    {
        return await _context.Classes
            .AnyAsync(c => c.SubjectId == subjectId);
    }

    public async Task<bool> IsSubjectCodeUniqueAsync(string subjectCode, int? excludeSubjectId = null)
    {
        var query = _context.Subjects.Where(s => s.SubjectCode == subjectCode);

        if (excludeSubjectId.HasValue)
            query = query.Where(s => s.SubjectId != excludeSubjectId.Value);

        return !await query.AnyAsync();
    }
}
