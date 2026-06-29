using CareerCounsellingApp.Data;
using CareerCounsellingApp.DTO;
using CareerCounsellingApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Services.Assessment;

public class AssessmentReportService : IAssessmentReportService
{
    private readonly AppDbContext _context;
    public AssessmentReportService(AppDbContext context)
    {
        _context = context;
    }
    public AssessmentReportDto GetReport(int assessmentId)
    {
        var assessmentResult = LoadAssessmentResult(assessmentId);
        return BuildReport(assessmentResult);

        // Mapping comes next...
    }
    private AssessmentResult LoadAssessmentResult(int assessmentId)
    {
        var result = _context.AssessmentResults
            .Include(r => r.Assessment)
                .ThenInclude(a => a.Student)
            .Include(r => r.ParentCategoryResults)
            .Include(r => r.CategoryResults)
                .ThenInclude(c => c.Category)
                    .ThenInclude(c => c.ParentCategory)
            .FirstOrDefault(r => r.AssessmentId == assessmentId);

        if (result == null)
            throw new Exception("Assessment result not found.");

        return result;
    }
    private AssessmentReportDto BuildReport(AssessmentResult result)
    {
        return new AssessmentReportDto
        {
            Student = BuildStudent(result),

            OverallScore = result.ObtainedScore,

            MaximumScore = result.MaximumScore,

            OverallPercentage = result.Percentage,

            OverallBand = result.Band,

            ParentCategories = BuildParentCategories(result),

            Categories = BuildCategories(result)
        };
    }
    private StudentInfoDto BuildStudent(AssessmentResult result)
    {
        var student = result.Assessment.Student;

        return new StudentInfoDto
        {
            StudentId = student.Id,

            StudentName = student.FullName,

            AdmissionNo = student.AdmissionNo,

            Course = student.Course,

            Gender = student.Gender,

            DateOfBirth = student.DateOfBirth,

            Email = student.Email,

            MobileNumber = student.MobileNumber,

            PhotoPath = student.PhotoPath,

            AssessmentDate = result.Assessment.AssessmentDate
        };
    }
    private List<CategoryResultDto> BuildCategories(
    AssessmentResult result)
    {
        return result.CategoryResults
            .Select(c => new CategoryResultDto
            {
                CategoryId = c.CategoryId,

                CategoryName = c.CategoryName,

                ObtainedScore = c.ObtainedScore,

                MaximumScore = c.MaximumScore,

                Percentage = c.Percentage,

                Band = c.Band
            })
            .ToList();
    }
    private List<ParentCategoryResultDto> BuildParentCategories(
    AssessmentResult result)
    {
        return result.ParentCategoryResults
            .Select(parent => new ParentCategoryResultDto
            {
                ParentCategoryId = parent.ParentCategoryId,

                ParentCategoryName = parent.ParentCategoryName,

                ObtainedScore = parent.ObtainedScore,

                MaximumScore = parent.MaximumScore,

                Percentage = parent.Percentage,

                Band = parent.Band,

                Categories = result.CategoryResults
                    .Where(c =>
                        c.Category.ParentCategoryId ==
                        parent.ParentCategoryId)
                    .Select(c => new CategoryResultDto
                    {
                        CategoryId = c.CategoryId,
                        CategoryName = c.CategoryName,
                        ObtainedScore = c.ObtainedScore,
                        MaximumScore = c.MaximumScore,
                        Percentage = c.Percentage,
                        Band = c.Band
                    })
                    .ToList()
            })
            .ToList();
    }
}
