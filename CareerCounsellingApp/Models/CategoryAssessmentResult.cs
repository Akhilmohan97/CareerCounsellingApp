using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Models
{
    public class CategoryAssessmentResult
    {
        public int Id { get; set; }

        public int AssessmentResultId { get; set; }

        public int CategoryId { get; set; }

        public decimal ObtainedScore { get; set; }

        public decimal MaximumScore { get; set; }

        public decimal Percentage { get; set; }

        public string Band { get; set; } = "";

        public AssessmentResult AssessmentResult { get; set; } = null!;

        public Category Category { get; set; } = null!;
    }
}
