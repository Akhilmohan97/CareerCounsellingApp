using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Models
{
    public class ParentCategoryScore
    {
        public string ParentCategoryName { get; set; }

        public List<CategoryScore> Categories { get; set; } = new();
    }
}
