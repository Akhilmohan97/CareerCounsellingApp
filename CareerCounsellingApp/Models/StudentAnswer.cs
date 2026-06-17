using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Models;

public class StudentAnswer
{
    public int Id { get; set; }

    public int AssessmentId { get; set; }

    public int QuestionId { get; set; }

    public int QuestionOptionId { get; set; }

    public Assessment? Assessment { get; set; }

    public Question? Question { get; set; }

    public QuestionOption? QuestionOption { get; set; }
}
