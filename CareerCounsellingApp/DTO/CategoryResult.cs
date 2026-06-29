using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Services.Assessment
{
    public class CategoryResult
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = "";

        public decimal ObtainedScore { get; set; }

        public decimal MaximumScore { get; set; }

        public decimal Percentage { get; set; }

        public string Band { get; set; } = "";
    }
}
