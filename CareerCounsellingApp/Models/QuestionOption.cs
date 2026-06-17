using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Models;

public class QuestionOption
{
    public int Id { get; set; }

    public int QuestionId { get; set; }

    public string OptionText { get; set; } = "";

    public int Score { get; set; }

    public Question? Question { get; set; }
}
