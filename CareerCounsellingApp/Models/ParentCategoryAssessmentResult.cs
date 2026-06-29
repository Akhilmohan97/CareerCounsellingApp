using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Models
{
    public class ParentCategoryAssessmentResult
    {
        public int Id { get; set; }

        public int AssessmentResultId { get; set; }

        public int ParentCategoryId { get; set; }

        public decimal ObtainedScore { get; set; }

        public decimal MaximumScore { get; set; }

        public decimal Percentage { get; set; }

        public string Band { get; set; } = "";

        public AssessmentResult AssessmentResult { get; set; } = null!;

        public ParentCategory ParentCategory { get; set; } = null!;
    }
}
