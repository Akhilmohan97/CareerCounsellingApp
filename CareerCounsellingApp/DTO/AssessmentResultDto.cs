using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.DTO
{
    public class AssessmentResultDto
    {
        public int AssessmentId { get; set; }

        public decimal OverallScore { get; set; }

        public decimal MaximumScore { get; set; }

        public decimal OverallPercentage { get; set; }

        public string OverallBand { get; set; } = "";

        public List<CategoryResultDto> CategoryResults { get; set; }
            = new();

        public List<ParentCategoryResultDto> ParentCategoryResults { get; set; }
            = new();
    }
}
