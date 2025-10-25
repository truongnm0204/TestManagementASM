using System;
using System.Collections.Generic;

namespace TestManagementASM.Models;

public partial class StudentAnswer
{
    public int StudentAnswerId { get; set; }

    public int AttemptId { get; set; }

    public int QuestionId { get; set; }

    public int ChosenAnswerId { get; set; }

    public virtual TestAttempt Attempt { get; set; } = null!;

    public virtual Answer ChosenAnswer { get; set; } = null!;

    public virtual Question Question { get; set; } = null!;
}
