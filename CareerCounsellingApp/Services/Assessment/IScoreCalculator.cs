using CareerCounsellingApp.DTO;
using CareerCounsellingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Services.Assessment
{
    public interface IScoreCalculator
    {
        CategoryResult CalculateCategory(
        Category category,
        IEnumerable<StudentAnswer> answers);

        ParentCategoryResult CalculateParentCategory(
            ParentCategory parentCategory,
            IEnumerable<CategoryResult> categoryResults);

        string GetBand(decimal percentage);
    }
}
