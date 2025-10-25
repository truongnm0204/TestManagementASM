using System;
using System.Collections.Generic;

namespace TestManagementASM.Models;

public partial class TestQuestion
{
    public int TestQuestionId { get; set; }

    public int TestId { get; set; }

    public int QuestionId { get; set; }

    public int Points { get; set; }

    public virtual Question Question { get; set; } = null!;

    public virtual Test Test { get; set; } = null!;
}
