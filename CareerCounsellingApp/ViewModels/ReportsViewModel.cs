using CareerCounsellingApp.Data;
using CareerCounsellingApp.Models;
using CareerCounsellingApp.Helpers;
using CareerCounsellingApp.DTO;
using CareerCounsellingApp.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CareerCounsellingApp.ViewModels;

public class ReportsViewModel : INotifyPropertyChanged
{
    private readonly AppDbContext _context;
    private DateTime _dateFrom = DateTime.Today.AddMonths(-1);
    private DateTime _dateTo = DateTime.Today;
    private string _selectedFilter = "All";

    public DateTime DateFrom
    {
        get => _dateFrom;
        set
        {
            if (_dateFrom == value) return;
            _dateFrom = value;
            OnPropertyChanged(nameof(DateFrom));
            RefreshReports();
        }
    }

    public DateTime DateTo
    {
        get => _dateTo;
        set
        {
            if (_dateTo == value) return;
            _dateTo = value;
            OnPropertyChanged(nameof(DateTo));
            RefreshReports();
        }
    }

    public string SelectedFilter
    {
        get => _selectedFilter;
        set
        {
            if (_selectedFilter == value) return;
            _selectedFilter = value;
            OnPropertyChanged(nameof(SelectedFilter));
            RefreshReports();
        }
    }

    // Summary Statistics
    private int _totalAssessments;
    public int TotalAssessments
    {
        get => _totalAssessments;
        set
        {
            if (_totalAssessments == value) return;
            _totalAssessments = value;
            OnPropertyChanged(nameof(TotalAssessments));
        }
    }

    private decimal _averageScore;
    public decimal AverageScore
    {
        get => _averageScore;
        set
        {
            if (_averageScore == value) return;
            _averageScore = value;
            OnPropertyChanged(nameof(AverageScore));
        }
    }

    private int _highPerformers;
    public int HighPerformers
    {
        get => _highPerformers;
        set
        {
            if (_highPerformers == value) return;
            _highPerformers = value;
            OnPropertyChanged(nameof(HighPerformers));
        }
    }

    private int _studentsNeedingSupport;
    public int StudentsNeedingSupport
    {
        get => _studentsNeedingSupport;
        set
        {
            if (_studentsNeedingSupport == value) return;
            _studentsNeedingSupport = value;
            OnPropertyChanged(nameof(StudentsNeedingSupport));
        }
    }

    // Performance Data
    public ObservableCollection<StudentPerformanceItem> StudentPerformance { get; } = new();
    
    public class StudentPerformanceItem
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string AdmissionNo { get; set; }
        public string Course { get; set; }
        public decimal Score { get; set; }
        public string Band { get; set; }
        public DateTime AssessmentDate { get; set; }
    }

    // Category Performance
    public ObservableCollection<CategoryPerformanceItem> CategoryPerformance { get; } = new();

    public class CategoryPerformanceItem
    {
        public string CategoryName { get; set; }
        public decimal AverageScore { get; set; }
        public int TotalAttempts { get; set; }
        public decimal HighestScore { get; set; }
        public decimal LowestScore { get; set; }
    }

    // Band Distribution
    public ObservableCollection<BandDistributionItem> BandDistribution { get; } = new();

    public class BandDistributionItem
    {
        public string Band { get; set; }
        public int Count { get; set; }
        public decimal Percentage { get; set; }
    }

    // Filter lists
    public ObservableCollection<string> FilterOptions { get; } = new()
    {
        "All",
        "Excellent",
        "High",
        "Moderate",
        "Developing",
        "Low"
    };

    // Open Report Command
    private Helpers.RelayCommand<StudentAssessmentSummary?>? _openReportCommand;
    public ICommand OpenReportCommand
    {
        get
        {
            if (_openReportCommand == null)
            {
                _openReportCommand = new Helpers.RelayCommand<StudentAssessmentSummary?>(OpenReport);
            }
            return _openReportCommand;
        }
    }

    // Results collection for display
    public ObservableCollection<StudentAssessmentSummary> Results { get; } = new();

    public ReportsViewModel()
    {
        _context = new AppDbContext();
        LoadReports();
    }

    public void LoadReports()
    {
        Task.Run(() => RefreshReports());
    }

    private void RefreshReports()
    {
        try
        {
            // Load assessment summary
            var assessments = _context.AssessmentResults
                .Include(ar => ar.Assessment)
                .ThenInclude(a => a.Student)
                .Where(ar => ar.GeneratedOn.Date >= DateFrom.Date && 
                             ar.GeneratedOn.Date <= DateTo.Date)
                .ToList();

            TotalAssessments = assessments.Count;
            AverageScore = assessments.Any() 
                ? Math.Round(assessments.Average(a => a.Percentage), 2) 
                : 0;

            HighPerformers = assessments.Count(a => a.Percentage >= 85);
            StudentsNeedingSupport = assessments.Count(a => a.Percentage < 40);

            // Load student performance
            LoadStudentPerformance(assessments);

            // Load category performance
            LoadCategoryPerformance();

            // Load band distribution
            LoadBandDistribution(assessments);

            // Load assessment results summary (for list display)
            LoadAssessmentResults();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading reports: {ex.Message}");
        }
    }

    private void LoadAssessmentResults()
    {
        Results.Clear();

        var results = _context.AssessmentResults
            .Include(r => r.Assessment)
            .ThenInclude(a => a.Student)
            .OrderByDescending(r => r.Assessment.AssessmentDate)
            .ToList();

        foreach (var result in results)
        {
            Results.Add(new StudentAssessmentSummary
            {
                AssessmentId = result.AssessmentId,
                StudentId = result.Assessment.StudentId,
                StudentName = result.Assessment.Student.FullName,
                AdmissionNo = result.Assessment.Student.AdmissionNo,
                Course = result.Assessment.Student.Course,
                AssessmentDate = result.Assessment.AssessmentDate,
                OverallPercentage = result.Percentage,
                OverallBand = result.Band
            });
        }
    }

    private void LoadStudentPerformance(List<AssessmentResult> assessments)
    {
        StudentPerformance.Clear();

        var performanceData = assessments
            .OrderByDescending(a => a.Percentage)
            .Select(a => new StudentPerformanceItem
            {
                StudentId = a.Assessment.Student.Id,
                StudentName = a.Assessment.Student.FullName,
                AdmissionNo = a.Assessment.Student.AdmissionNo,
                Course = a.Assessment.Student.Course,
                Score = a.Percentage,
                Band = a.Band,
                AssessmentDate = a.GeneratedOn
            })
            .ToList();

        foreach (var item in performanceData)
        {
            // Apply filter
            if (SelectedFilter != "All" && item.Band != SelectedFilter)
                continue;

            StudentPerformance.Add(item);
        }
    }

    private void LoadCategoryPerformance()
    {
        CategoryPerformance.Clear();

        var categoryResults = _context.CategoryAssessmentResults
            .Include(c => c.Category)
            .Where(c => c.AssessmentResult.GeneratedOn.Date >= DateFrom.Date &&
                        c.AssessmentResult.GeneratedOn.Date <= DateTo.Date)
            .GroupBy(c => c.CategoryName)
            .Select(g => new CategoryPerformanceItem
            {
                CategoryName = g.Key,
                AverageScore = Math.Round(g.Average(x => x.Percentage), 2),
                TotalAttempts = g.Count(),
                HighestScore = g.Max(x => x.Percentage),
                LowestScore = g.Min(x => x.Percentage)
            })
            .OrderByDescending(x => x.AverageScore)
            .ToList();

        foreach (var item in categoryResults)
        {
            CategoryPerformance.Add(item);
        }
    }

    private void LoadBandDistribution(List<AssessmentResult> assessments)
    {
        BandDistribution.Clear();

        var bands = new[] { "Excellent", "High", "Moderate", "Developing", "Low" };
        var total = assessments.Count;

        foreach (var band in bands)
        {
            var count = assessments.Count(a => a.Band == band);
            var percentage = total > 0 ? Math.Round((count * 100m) / total, 2) : 0;

            BandDistribution.Add(new BandDistributionItem
            {
                Band = band,
                Count = count,
                Percentage = percentage
            });
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void OpenReport(StudentAssessmentSummary? summary)
    {
        if (summary == null)
            return;

        var window = new AssessmentResultWindow(summary.AssessmentId);
        window.Show();
    }
}
