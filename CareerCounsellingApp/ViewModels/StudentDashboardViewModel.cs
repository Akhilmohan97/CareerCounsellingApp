using CareerCounsellingApp.Data;
using CareerCounsellingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.ViewModels;

public class StudentDashboardViewModel
{
    public string WelcomeMessage { get; }

    public User CurrentUser { get; }

    public Student? CurrentStudent { get; }

    public StudentDashboardViewModel(User user)
    {
        CurrentUser = user;

        using var db = new AppDbContext();

        CurrentStudent = db.Students
            .FirstOrDefault(x => x.UserId == user.Id);

        WelcomeMessage =
            $"Welcome {CurrentStudent?.FullName ?? user.Username}";
    }
}
