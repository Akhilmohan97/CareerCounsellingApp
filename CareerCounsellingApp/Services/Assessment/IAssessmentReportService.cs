using CareerCounsellingApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Services.Assessment
{
    public interface IAssessmentReportService
    {
        AssessmentReportDto GetReport(int assessmentId);
    }
}
