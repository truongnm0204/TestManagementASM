using System;
using System.Collections.Generic;

namespace TestManagementASM.Models;

public partial class Test
{
    public int TestId { get; set; }

    public int ClassId { get; set; }

    public int CreatedByTeacherId { get; set; }

    public string TestName { get; set; } = null!;

    public int? DurationMinutes { get; set; }

    public bool IsActive { get; set; }

    public DateTime? AvailableFrom { get; set; }

    public DateTime? AvailableTo { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual User CreatedByTeacher { get; set; } = null!;

    public virtual ICollection<TestAttempt> TestAttempts { get; set; } = new List<TestAttempt>();

    public virtual ICollection<TestQuestion> TestQuestions { get; set; } = new List<TestQuestion>();
}
