using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Models
{
    public class AssessmentResult
    {
        public int Id { get; set; }

        public int AssessmentId { get; set; }

        public decimal ObtainedScore { get; set; }

        public decimal MaximumScore { get; set; }

        public decimal Percentage { get; set; }

        public string Band { get; set; } = "";

        public DateTime GeneratedOn { get; set; }

        public Assessment Assessment { get; set; } = null!;

        public ICollection<CategoryAssessmentResult> CategoryResults { get; set; }
            = new List<CategoryAssessmentResult>();

        public ICollection<ParentCategoryAssessmentResult> ParentCategoryResults { get; set; }
            = new List<ParentCategoryAssessmentResult>();
        public AIInterpretation? AIInterpretation { get; set; }
    }
}
