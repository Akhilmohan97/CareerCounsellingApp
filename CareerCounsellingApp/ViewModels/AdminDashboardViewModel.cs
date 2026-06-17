using CareerCounsellingApp.Data;
using System.ComponentModel;
using System.Linq;

namespace CareerCounsellingApp.ViewModels;

public class AdminDashboardViewModel : INotifyPropertyChanged
{
    private int _totalStudents;
    private int _totalQuestions;
    private int _totalCategories;
    private int _studentsAssessed;
    private int _totalAssessments;

    public int TotalStudents
    {
        get => _totalStudents;
        set
        {
            _totalStudents = value;
            OnPropertyChanged(nameof(TotalStudents));
        }
    }

    public int TotalQuestions
    {
        get => _totalQuestions;
        set
        {
            _totalQuestions = value;
            OnPropertyChanged(nameof(TotalQuestions));
        }
    }

    public int TotalCategories
    {
        get => _totalCategories;
        set
        {
            _totalCategories = value;
            OnPropertyChanged(nameof(TotalCategories));
        }
    }

    public int StudentsAssessed
    {
        get => _studentsAssessed;
        set
        {
            _studentsAssessed = value;
            OnPropertyChanged(nameof(StudentsAssessed));
        }
    }

    public int TotalAssessments
    {
        get => _totalAssessments;
        set
        {
            _totalAssessments = value;
            OnPropertyChanged(nameof(TotalAssessments));
        }
    }

    public AdminDashboardViewModel()
    {
        LoadStatistics();
    }

    public void LoadStatistics()
    {
        using var db = new AppDbContext();

        TotalStudents = db.Students.Count();
        TotalQuestions = db.Questions.Count();
        TotalCategories = db.Categories.Count();
        TotalAssessments = db.Assessments.Count();
        
        // Count unique students who have taken assessments
        StudentsAssessed = db.Assessments
            .Select(a => a.StudentId)
            .Distinct()
            .Count();
    }

    public void ReloadStatistics()
    {
        LoadStatistics();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

