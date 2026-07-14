using CareerCounsellingApp.PhotoCapture.Interfaces;
using CareerCounsellingApp.PhotoCapture.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.PhotoCapture.Services
{
    public class PhotoCaptureService : IPhotoCaptureService
    {
        public event Action<CaptureSession>? SessionUpdated;
        private readonly Dictionary<Guid, CaptureSession> _sessions = new();
        public Task CompleteSessionAsync(Guid sessionId, string imagePath)
        {
            var session = GetSession(sessionId);

            if (session != null)
            {
                session.PhotoPath = imagePath;
            }

            return Task.CompletedTask;
        }
        private void RaiseUpdated(CaptureSession session)
        {
            SessionUpdated?.Invoke(session);
        }
        public CaptureSession? GetSession(Guid sessionId)
        {
            _sessions.TryGetValue(sessionId, out var session);
            if (session?.IsExpired==true)
            {
                _sessions.Remove(sessionId);
                return null;
            }
            return session;
        }

        public Task<CaptureSession> StartSessionAsync()
        {
            var session = new CaptureSession();
            _sessions[session.SessionId] = session;
            return Task.FromResult(session);
        }
    }
}
