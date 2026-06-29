using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Services.Assessment
{
    public class ParentCategoryResult
    {
        public int ParentCategoryId { get; set; }

        public string ParentCategoryName { get; set; } = "";

        public decimal ObtainedScore { get; set; }

        public decimal MaximumScore { get; set; }

        public decimal Percentage { get; set; }

        public string Band { get; set; } = "";

        public List<CategoryResult> Categories { get; set; }
            = new();
    }
}
