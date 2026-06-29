using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Services.Assessment.Contracts
{
    public interface IAssessmentEngine
    {
        Task<AssessmentResult> CalculateAsync(int assessmentId);
    }
}
