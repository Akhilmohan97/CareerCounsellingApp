using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.DTO
{
    public class AssessmentReportDto
    {
        public StudentInfoDto Student { get; set; } = new();

        public decimal OverallScore { get; set; }

        public decimal MaximumScore { get; set; }

        public decimal OverallPercentage { get; set; }

        public string OverallBand { get; set; } = "";

        public List<ParentCategoryResultDto> ParentCategories { get; set; } = new();


        public string OverallRemark =>
    OverallPercentage switch
    {
        >= 90 => "Outstanding overall performance.",
        >= 75 => "Very good overall performance.",
        >= 60 => "Good performance with room for improvement.",
        >= 40 => "Needs additional guidance in several areas.",
        _ => "Requires significant counselling support."
    };
    }
}
