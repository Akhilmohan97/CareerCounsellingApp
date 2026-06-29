using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Models
{
    public class Question
    {
        public int Id { get; set; }

        public string QuestionText { get; set; } = "";
        public string QuestionTextMalayalam { get; set; } = "";
        public int CategoryId { get; set; }
        public decimal MaximumScore { get; set; }
        public Category? Category { get; set; }
        public ICollection<QuestionOption> Options { get; set; }
       = new List<QuestionOption>();
    }
}
