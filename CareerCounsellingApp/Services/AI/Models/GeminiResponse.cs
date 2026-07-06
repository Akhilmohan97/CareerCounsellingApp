using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Services.AI.Models;

public class GeminiResponse
{
    public List<Candidate> Candidates { get; set; } = new();
}
