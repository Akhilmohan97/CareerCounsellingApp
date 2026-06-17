using CareerCounsellingApp.Data;
using CareerCounsellingApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.ViewModels
{
    public class AssessmentResultsViewModel : INotifyPropertyChanged
    {
        private StudentAssessmentResult? _selectedResult;

        public ObservableCollection<StudentAssessmentResult> Results { get; } = new();

        public ObservableCollection<CategoryScore> CategoryScores { get; } = new();

        private string _highestCategory = "";

        public string HighestCategory
        {
            get => _highestCategory;
            set
            {
                _highestCategory = value;
                OnPropertyChanged(nameof(HighestCategory));
            }
        }

        public StudentAssessmentResult? SelectedResult
        {
            get => _selectedResult;
            set
            {
                _selectedResult = value;

                OnPropertyChanged(nameof(SelectedResult));

                if (value != null)
                {
                    LoadCategoryScores(value.AssessmentId);
                }
            }
        }

        public AssessmentResultsViewModel()
        {
            LoadResults();
        }

        private void LoadResults()
        {
            using var db = new AppDbContext();

            Results.Clear();

            var results =
                from assessment in db.Assessments
                join student in db.Students
                    on assessment.StudentId equals student.Id
                select new StudentAssessmentResult
                {
                    StudentId = student.Id,
                    StudentName = student.FullName,
                    AssessmentId = assessment.Id,
                    AssessmentDate = assessment.AssessmentDate
                };

            foreach (var result in results)
            {
                Results.Add(result);
            }
        }

        private void LoadCategoryScores(int assessmentId)
        {
            using var db = new AppDbContext();

            CategoryScores.Clear();

            var scores =
                from answer in db.StudentAnswers
                join option in db.QuestionOptions
                    on answer.QuestionOptionId equals option.Id
                join question in db.Questions
                    on answer.QuestionId equals question.Id
                join category in db.Categories
                    on question.CategoryId equals category.Id
                where answer.AssessmentId == assessmentId
                group option by category.Name into g
                select new CategoryScore
                {
                    CategoryName = g.Key,
                    Score = g.Sum(x => x.Score)
                };

            foreach (var score in scores)
            {
                CategoryScores.Add(score);
            }

            var highest =
                CategoryScores
                .OrderByDescending(x => x.Score)
                .FirstOrDefault();

            HighestCategory =
                highest?.CategoryName ?? "";
        }

        public event PropertyChangedEventHandler?
            PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }
    }
}
