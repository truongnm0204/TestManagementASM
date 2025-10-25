using System;
using System.Collections.Generic;

namespace TestManagementASM.Models;

public partial class Subject
{
    public int SubjectId { get; set; }

    public string SubjectCode { get; set; } = null!;

    public string SubjectName { get; set; } = null!;

    public int? CreatedByUserId { get; set; }

    public bool Status { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual User? CreatedByUser { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
