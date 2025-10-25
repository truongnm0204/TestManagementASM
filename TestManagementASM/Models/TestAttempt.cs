using System;
using System.Collections.Generic;

namespace TestManagementASM.Models;

public partial class TestAttempt
{
    public int AttemptId { get; set; }

    public int StudentId { get; set; }

    public int TestId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public int? Score { get; set; }

    public string AttemptStatus { get; set; } = null!;

    public virtual User Student { get; set; } = null!;

    public virtual ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();

    public virtual Test Test { get; set; } = null!;
}
