using System;
using System.Collections.Generic;

namespace TestManagementASM.Models;

public partial class Class
{
    public int ClassId { get; set; }

    public int SubjectId { get; set; }

    public string ClassName { get; set; } = null!;

    public string? Semester { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual Subject Subject { get; set; } = null!;

    public virtual ICollection<TeachingAssignment> TeachingAssignments { get; set; } = new List<TeachingAssignment>();

    public virtual ICollection<Test> Tests { get; set; } = new List<Test>();
}
