using CareerCounsellingApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace CareerCounsellingApp.Data
{
    public class AppDbContext: DbContext
    {

        public DbSet<User> Users => Set<User>();

        public DbSet<Category> Categories => Set<Category>();

        public DbSet<Question> Questions => Set<Question>();
        public DbSet<QuestionImage> QuestionImages => Set<QuestionImage>();
        public DbSet<QuestionOption> QuestionOptions => Set<QuestionOption>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Assessment> Assessments => Set<Assessment>();

        public DbSet<StudentAnswer> StudentAnswers => Set<StudentAnswer>();
        public DbSet<ParentCategory> ParentCategories => Set<ParentCategory>();
        public DbSet<AssessmentResult> AssessmentResults { get; set; }
        public DbSet<ParentCategoryAssessmentResult> ParentCategoryAssessmentResults { get; set; }
        public DbSet<CategoryAssessmentResult> CategoryAssessmentResults { get; set; }
        public DbSet<AIInterpretation> AIInterpretations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           

            // Priority: environment variable -> appsettings.json (ConnectionStrings:Default)
           

            // Try to load from appsettings.json in the application base directory
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = config.GetConnectionString("Default");


            optionsBuilder.UseNpgsql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>()
                .HasOne(q => q.Image)
                .WithOne(qi => qi.Question)
                .HasForeignKey<QuestionImage>(qi => qi.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
