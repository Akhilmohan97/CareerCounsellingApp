using CareerCounsellingApp.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace CareerCounsellingApp.Data
{
    public class AppDbContext: DbContext
    {
        private const string NeonConnectionStringEnvironmentVariable = "NEON_CONNECTION_STRING";

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
            if (optionsBuilder.IsConfigured)
            {
                return;
            }

            var connectionString = "postgresql://neondb_owner:npg_3kiop2vtJxcA@ep-nameless-mud-azz2h6ja-pooler.c-3.ap-southeast-1.aws.neon.tech/neondb?sslmode=require&channel_binding=require";

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    $"Set the '{NeonConnectionStringEnvironmentVariable}' environment variable with the Neon PostgreSQL connection string.");
            }

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
