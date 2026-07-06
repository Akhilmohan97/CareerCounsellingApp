using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Services.AI;

public class GeminiSettings
{
    public string ApiKey { get; set; }

    public string Model { get; set; } = "gemini-2.5-flash";
}
