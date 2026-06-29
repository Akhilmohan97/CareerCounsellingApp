using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.DTO
{
    public class AssessmentResult
    {
        public int AssessmentId { get; set; }

        public decimal OverallScore { get; set; }

        public decimal MaximumScore { get; set; }

        public decimal OverallPercentage { get; set; }

        public string OverallBand { get; set; } = "";

        public List<CategoryResult> CategoryResults { get; set; }
            = new();

        public List<ParentCategoryResult> ParentCategoryResults { get; set; }
            = new();
    }
}
