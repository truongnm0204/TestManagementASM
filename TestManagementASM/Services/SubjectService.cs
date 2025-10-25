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
            _context.Subjects.Update(subject);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
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
}
