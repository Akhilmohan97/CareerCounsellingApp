using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.DTO
{
    public class CategoryResultDto
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = "";
        public int ParentCategoryId { get; set; }

        public string ParentCategoryName { get; set; } = string.Empty;
        public decimal ObtainedScore { get; set; }

        public decimal MaximumScore { get; set; }

        public decimal Percentage { get; set; }

        public string Band { get; set; } = "";
    }
}
