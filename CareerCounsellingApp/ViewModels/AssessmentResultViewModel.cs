using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.ViewModels
{
    public class AssessmentResultViewModel
    {
        public int AssessmentId { get; }

        public AssessmentResultViewModel(int assessmentId)
        {
            AssessmentId = assessmentId;
        }
    }
}
