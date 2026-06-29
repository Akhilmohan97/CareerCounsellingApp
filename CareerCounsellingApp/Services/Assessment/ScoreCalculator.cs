using CareerCounsellingApp.DTO;
using CareerCounsellingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Services.Assessment
{
    public class ScoreCalculator : IScoreCalculator
    {
        public CategoryResult CalculateCategory(Category category, IEnumerable<StudentAnswer> answers)
        {
            throw new NotImplementedException();
        }

        public ParentCategoryResult CalculateParentCategory(ParentCategory parentCategory, IEnumerable<CategoryResult> categoryResults)
        {
            throw new NotImplementedException();
        }

        public string GetBand(decimal percentage)
        {
            if (percentage >= 85)
                return "Excellent";

            if (percentage >= 70)
                return "High";

            if (percentage >= 55)
                return "Moderate";

            if (percentage >= 40)
                return "Developing";

            return "Low";
        }
    }
}
