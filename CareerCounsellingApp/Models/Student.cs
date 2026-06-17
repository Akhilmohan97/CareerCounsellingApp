using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Models;

public class Student
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string AdmissionNo { get; set; } = "";

    public string FullName { get; set; } = "";

    public DateTime? DateOfBirth { get; set; }

    public string Gender { get; set; } = "";

    public string Email { get; set; } = "";

    public string MobileNumber { get; set; } = "";

    public string Course { get; set; } = "";

    public string Institution { get; set; } = "";

    public string PhotoPath { get; set; } = "";

    public User? User { get; set; }
}
