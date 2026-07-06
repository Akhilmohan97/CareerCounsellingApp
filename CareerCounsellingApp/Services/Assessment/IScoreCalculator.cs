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
        CategoryResultDto CalculateCategory(
        Category category,
        IEnumerable<StudentAnswer> answers);

        ParentCategoryResultDto CalculateParentCategory(
            ParentCategory parentCategory,
            IEnumerable<CategoryResultDto> categoryResults);

        string GetBand(decimal percentage);
    }
}
