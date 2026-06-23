using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Helpers
{
    public static class VersionHelper
    {
        public static string CurrentVersion => Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Unknown";
    }
}
