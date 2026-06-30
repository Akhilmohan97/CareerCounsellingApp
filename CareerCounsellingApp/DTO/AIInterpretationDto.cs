using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.DTO
{
    public class AIInterpretationDto
    {
        public string ExecutiveSummary { get; set; } = "";

        public List<string> Strengths { get; set; } = new();

        public List<string> DevelopmentAreas { get; set; } = new();

        public List<string> DiscussionPoints { get; set; } = new();
    }
}
