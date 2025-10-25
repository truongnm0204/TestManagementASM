using System;
using System.Collections.Generic;

namespace TestManagementASM.Models;

public partial class Question
{
    public int QuestionId { get; set; }

    public int SubjectId { get; set; }

    public int? CreatedByTeacherId { get; set; }

    public string QuestionText { get; set; } = null!;

    public string QuestionType { get; set; } = null!;

    public int? DifficultyLevel { get; set; }

    public int? Chapter { get; set; }

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual User? CreatedByTeacher { get; set; }

    public virtual ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();

    public virtual Subject Subject { get; set; } = null!;

    public virtual ICollection<TestQuestion> TestQuestions { get; set; } = new List<TestQuestion>();
}
