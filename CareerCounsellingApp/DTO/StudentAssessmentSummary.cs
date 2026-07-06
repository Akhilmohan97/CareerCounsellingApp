using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.DTO;

public class StudentAssessmentSummary
{
    public int AssessmentId { get; set; }

    public int StudentId { get; set; }

    public string StudentName { get; set; } = "";

    public string AdmissionNo { get; set; } = "";

    public string Course { get; set; } = "";

    public DateTime AssessmentDate { get; set; }

    public decimal OverallPercentage { get; set; }

    public string OverallBand { get; set; } = "";
}
