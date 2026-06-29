using CareerCounsellingApp.Data;
using CareerCounsellingApp.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Services.Assessment;

public class AssessmentEngine : IAssessmentEngine
{
    private readonly AppDbContext _context;
    private readonly IScoreCalculator _scoreCalculator;
    public AssessmentEngine(
        AppDbContext context,
        IScoreCalculator scoreCalculator)
    {
        _context = context;
        _scoreCalculator = scoreCalculator;
    }
    public async Task<AssessmentResult> CalculateAsync(int assessmentId)
    {
        var assessment = await _context.Assessments
                        .Include(a => a.Answers)
                            .ThenInclude(sa => sa.Question)
                                .ThenInclude(q => q.Category)
                                    .ThenInclude(c => c.ParentCategory)
                        .Include(a => a.Answers)
                            .ThenInclude(sa => sa.QuestionOption)
                        .FirstOrDefaultAsync(a => a.Id == assessmentId);

        if (assessment == null)
            throw new Exception("Assessment not found.");

        var categories = assessment.Answers
                        .Select(a => a.Question!.Category!)
                        .DistinctBy(c => c.Id)
                        .ToList();
        var categoryResults = categories
                                .Select(category =>
                                    _scoreCalculator.CalculateCategory(
                                        category,
                                        assessment.Answers))
                                .ToList();
        var parentCategories = categories
                                .Select(c => c.ParentCategory)
                                .DistinctBy(p => p.Id)
                                .ToList();
        var parentResults = parentCategories
                                .Select(parent =>
                                    _scoreCalculator.CalculateParentCategory(
                                        parent,
                                        categoryResults))
                                .ToList();
        decimal obtained = categoryResults.Sum(c => c.ObtainedScore);

        decimal maximum = categoryResults.Sum(c => c.MaximumScore);

        decimal percentage = maximum == 0
            ? 0
            : Math.Round((obtained / maximum) * 100, 2);
        return new AssessmentResult
        {
            AssessmentId = assessment.Id,

            OverallScore = obtained,

            MaximumScore = maximum,

            OverallPercentage = percentage,

            OverallBand = _scoreCalculator.GetBand(percentage),

            CategoryResults = categoryResults,

            ParentCategoryResults = parentResults
        };
    }
}
