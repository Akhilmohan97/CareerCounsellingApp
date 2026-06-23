using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Models
{
    public class UpdateModel
    {
        public string CurrentVersion { get; set; } = "";

        public string LatestVersion { get; set; } = "";

        public string ReleaseNotes { get; set; } = "";

        public double Progress { get; set; }

        public bool IsDownloading { get; set; }

        public bool UpdateAvailable { get; set; }
    }
}
