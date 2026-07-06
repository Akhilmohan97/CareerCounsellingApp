using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Models
{
    public class AIInterpretation
    {
        public int Id { get; set; }

        public int AssessmentResultId { get; set; }

        public string ExecutiveSummary { get; set; } = "";

        public string StrengthsJson { get; set; } = "";

        public string DevelopmentAreasJson { get; set; } = "";

        public string DiscussionPointsJson { get; set; } = "";

        public string ModelName { get; set; } = "";

        public DateTime GeneratedOn { get; set; }

        public AssessmentResult AssessmentResult { get; set; } = null!;
    }
}
