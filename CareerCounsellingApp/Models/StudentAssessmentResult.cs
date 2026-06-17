using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Models;

public class StudentAssessmentResult
{
    public int StudentId { get; set; }

    public string StudentName { get; set; } = "";

    public int AssessmentId { get; set; }

    public DateTime AssessmentDate { get; set; }
}
