using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Models;

public class Assessment
{
        public int Id { get; set; }

        public int StudentId { get; set; }

        public DateTime AssessmentDate { get; set; }

        public Student? Student { get; set; }

        public ICollection<StudentAnswer> Answers { get; set; }
            = new List<StudentAnswer>();
    
}
