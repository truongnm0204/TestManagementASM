using Microsoft.EntityFrameworkCore;
using TestManagementASM.Models;
using TestManagementASM.Services.Interfaces;

namespace TestManagementASM.Services;

public class TeachingAssignmentService : ITeachingAssignmentService
{
    private readonly TestManagementDbContext _context;

    public TeachingAssignmentService(TestManagementDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetTeachersByClassAsync(int classId)
    {
        return await _context.TeachingAssignments
            .Where(ta => ta.ClassId == classId)
            .Include(ta => ta.Teacher)
            .Select(ta => ta.Teacher)
            .ToListAsync();
    }

    public async Task<List<Class>> GetClassesByTeacherAsync(int teacherId)
    {
        return await _context.TeachingAssignments
            .Where(ta => ta.TeacherId == teacherId)
            .Include(ta => ta.Class)
                .ThenInclude(c => c.Subject)
            .Select(ta => ta.Class)
            .ToListAsync();
    }

    public async Task<bool> AssignTeacherToClassAsync(int teacherId, int classId)
    {
        try
        {
            // Check if already assigned
            var exists = await _context.TeachingAssignments
                .AnyAsync(ta => ta.TeacherId == teacherId && ta.ClassId == classId);
            
            if (exists)
                return false;

            var assignment = new TeachingAssignment
            {
                TeacherId = teacherId,
                ClassId = classId
            };

            _context.TeachingAssignments.Add(assignment);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> RemoveTeacherFromClassAsync(int teacherId, int classId)
    {
        try
        {
            var assignment = await _context.TeachingAssignments
                .FirstOrDefaultAsync(ta => ta.TeacherId == teacherId && ta.ClassId == classId);
            
            if (assignment == null)
                return false;

            _context.TeachingAssignments.Remove(assignment);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> IsTeacherAssignedAsync(int teacherId, int classId)
    {
        return await _context.TeachingAssignments
            .AnyAsync(ta => ta.TeacherId == teacherId && ta.ClassId == classId);
    }
}

