using Microsoft.EntityFrameworkCore;
using TestManagementASM.Models;
using TestManagementASM.Services.Interfaces;

namespace TestManagementASM.Services;

public class ClassService : IClassService
{
    private readonly TestManagementDbContext _context;

    public ClassService(TestManagementDbContext context)
    {
        _context = context;
    }

    public async Task<List<Class>> GetTeacherClassesAsync(int teacherId)
    {
        return await _context.TeachingAssignments
            .Where(ta => ta.TeacherId == teacherId)
            .Include(ta => ta.Class)
                .ThenInclude(c => c.Subject)
            .Include(ta => ta.Class.Enrollments)
            .Include(ta => ta.Class.Tests)
            .Select(ta => ta.Class)
            .ToListAsync();
    }

    public async Task<Class?> GetClassByIdAsync(int classId)
    {
        return await _context.Classes
            .Include(c => c.Subject)
            .Include(c => c.Enrollments)
            .Include(c => c.Tests)
            .FirstOrDefaultAsync(c => c.ClassId == classId);
    }

    public async Task<List<User>> GetStudentsByClassAsync(int classId)
    {
        return await _context.Enrollments
            .Where(e => e.ClassId == classId)
            .Include(e => e.Student)
            .Select(e => e.Student)
            .ToListAsync();
    }

    public async Task<List<Class>> GetAllClassesAsync()
    {
        return await _context.Classes
            .Include(c => c.Subject)
            .Include(c => c.Enrollments)
            .Include(c => c.TeachingAssignments)
                .ThenInclude(ta => ta.Teacher)
            .OrderByDescending(c => c.ClassId)
            .ToListAsync();
    }

    public async Task<bool> AddClassAsync(Class @class)
    {
        try
        {
            _context.Classes.Add(@class);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateClassAsync(Class @class)
    {
        try
        {
            _context.Classes.Update(@class);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteClassAsync(int classId)
    {
        try
        {
            var @class = await _context.Classes.FindAsync(classId);
            if (@class == null)
                return false;

            _context.Classes.Remove(@class);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}

