using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.PhotoCapture.Models
{
    public enum CaptureState
    {
        Created,
        QRDisplayed,
        WaitingForUpload,
        Uploading,
        Completed,
        Expired
    }
}
