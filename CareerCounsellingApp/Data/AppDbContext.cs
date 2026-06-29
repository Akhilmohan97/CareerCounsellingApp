using CareerCounsellingApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace CareerCounsellingApp.Data
{
    public class AppDbContext: DbContext
    {
        public DbSet<User> Users => Set<User>();

        public DbSet<Category> Categories => Set<Category>();

        public DbSet<Question> Questions => Set<Question>();
        public DbSet<QuestionOption> QuestionOptions => Set<QuestionOption>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Assessment> Assessments => Set<Assessment>();

        public DbSet<StudentAnswer> StudentAnswers => Set<StudentAnswer>();
        public DbSet<ParentCategory> ParentCategories => Set<ParentCategory>();
        public DbSet<AssessmentResult> AssessmentResults { get; set; }
        public DbSet<ParentCategoryAssessmentResult> ParentCategoryAssessmentResults { get; set; }
        public DbSet<CategoryAssessmentResult> CategoryAssessmentResults { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string folder = Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                "CareerCounselling_v1");

            Directory.CreateDirectory(folder);

            string dbPath = Path.Combine(folder, "career.db");

            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }
}
