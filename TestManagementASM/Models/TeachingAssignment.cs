using System;
using System.Collections.Generic;

namespace TestManagementASM.Models;

public partial class TeachingAssignment
{
    public int AssignmentId { get; set; }

    public int TeacherId { get; set; }

    public int ClassId { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual User Teacher { get; set; } = null!;
}
