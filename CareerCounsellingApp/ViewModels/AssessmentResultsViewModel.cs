using CareerCounsellingApp.Data;
using CareerCounsellingApp.DTO;
using CareerCounsellingApp.Helpers;
using CommunityToolkit.Mvvm.Input;
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

public class AssessmentResultsViewModel : INotifyPropertyChanged
{
    public ICommand OpenReportCommand { get; }
    public ObservableCollection<StudentAssessmentSummary> Results { get; }
       = new();
    public int HighPerformerCount =>
    Results.Count(r => r.OverallPercentage >= 75);

    public decimal AverageScore =>
        Results.Any()
            ? Math.Round(Results.Average(r => r.OverallPercentage), 1)
            : 0;
    private StudentAssessmentSummary? _selectedResult;

    public StudentAssessmentSummary? SelectedResult
    {
        get => _selectedResult;
        set
        {
            _selectedResult = value;
            OnPropertyChanged(nameof(SelectedResult));
        }
    }

    public AssessmentResultsViewModel()
    {
        OpenReportCommand = new RelayCommand<StudentAssessmentSummary?>(OpenReport);
        LoadResults();
    }
    private void OpenReport(StudentAssessmentSummary? summary)
    {
        if (summary == null)
            return;

        var window = new AssessmentResultWindow(summary.AssessmentId);

        window.Show();
    }
    private void LoadResults()
    {
        using var db = new AppDbContext();

        Results.Clear();

        var results = db.AssessmentResults
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
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}
