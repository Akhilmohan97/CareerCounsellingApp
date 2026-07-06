using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.DTO;

public class StudentInfoDto
{
    public int StudentId { get; set; }

    public string StudentName { get; set; } = "";

    public string AdmissionNo { get; set; } = "";

    public string Course { get; set; } = "";

    public string Gender { get; set; } = "";

    public DateTime DateOfBirth { get; set; }

    public int Age
    {
        get
        {
            var age = DateTime.Today.Year - DateOfBirth.Year;

            if (DateOfBirth.Date > DateTime.Today.AddYears(-age))
                age--;

            return age;
        }
    }

    public string Email { get; set; } = "";

    public string MobileNumber { get; set; } = "";

    public string? PhotoPath { get; set; }

    public DateTime AssessmentDate { get; set; }
}
