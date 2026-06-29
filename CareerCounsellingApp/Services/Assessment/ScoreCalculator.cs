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
            var categoryAnswers = answers
                .Where(a => a.Question!.CategoryId == category.Id)
                .ToList();
            decimal obtainedScore = categoryAnswers
                .Sum(a => (decimal)a.QuestionOption!.Score);
            decimal maximumScore = categoryAnswers
                .Sum(a => a.Question!.MaximumScore);
            decimal percentage = 0;

            if (maximumScore > 0)
            {
                percentage = Math.Round(
                    (obtainedScore / maximumScore) * 100,
                    2);
            }
            return new CategoryResult
            {
                CategoryId = category.Id,

                CategoryName = category.Name,

                ParentCategoryId = category.ParentCategoryId,

                ParentCategoryName = category.ParentCategory.Name,

                ObtainedScore = obtainedScore,

                MaximumScore = maximumScore,

                Percentage = percentage,

                Band = GetBand(percentage)
            };

        }

        public ParentCategoryResult CalculateParentCategory(ParentCategory parentCategory, IEnumerable<CategoryResult> categoryResults)
        {
            var categories = categoryResults
                .Where(c => c.ParentCategoryId == parentCategory.Id)
                .ToList();
            decimal obtainedScore = categories
                .Sum(c => c.ObtainedScore);
            decimal maximumScore = categories
                .Sum(c => c.MaximumScore);
            decimal percentage = 0;

            if (maximumScore > 0)
            {
                percentage = Math.Round(
                    (obtainedScore / maximumScore) * 100,
                    2);
            }
            return new ParentCategoryResult
            {
                ParentCategoryId = parentCategory.Id,

                ParentCategoryName = parentCategory.Name,

                ObtainedScore = obtainedScore,

                MaximumScore = maximumScore,

                Percentage = percentage,

                Band = GetBand(percentage),

                Categories = categories
            };
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
