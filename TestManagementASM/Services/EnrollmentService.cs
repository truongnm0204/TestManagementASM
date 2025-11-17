using Microsoft.EntityFrameworkCore;
using TestManagementASM.Models;
using TestManagementASM.Services.Interfaces;

namespace TestManagementASM.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly TestManagementDbContext _context;

    public EnrollmentService(TestManagementDbContext context)
    {
        _context = context;
    }

    public async Task<List<Class>> GetStudentClassesAsync(int studentId)
    {
        return await _context.Enrollments
            .Where(e => e.StudentId == studentId)
            .Include(e => e.Class)
                .ThenInclude(c => c.Subject)
            .Select(e => e.Class)
            .ToListAsync();
    }

    public async Task<bool> IsStudentEnrolledAsync(int studentId, int classId)
    {
        return await _context.Enrollments
            .AnyAsync(e => e.StudentId == studentId && e.ClassId == classId);
    }
}

