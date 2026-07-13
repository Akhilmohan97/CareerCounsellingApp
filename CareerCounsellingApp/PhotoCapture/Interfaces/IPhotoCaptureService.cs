using CareerCounsellingApp.PhotoCapture.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.PhotoCapture.Interfaces
{
    internal interface IPhotoCaptureService
    {
        Task<CaptureSession> StartSessionAsync(int studentId);

        Task CompleteSessionAsync(Guid sessionId, string imagePath);

        CaptureSession? GetSession(Guid sessionId);
    }
}
