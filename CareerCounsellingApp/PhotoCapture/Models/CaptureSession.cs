using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.PhotoCapture.Models
{
    public sealed class CaptureSession
    {
        public Guid SessionId { get; init; } = Guid.NewGuid();
        public CaptureState State { get; set; } = CaptureState.Created;
        public int StudentId { get; init; }

        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

        public DateTime ExpiresAt => CreatedAt.AddMinutes(5);

        public bool IsExpired => DateTime.UtcNow > ExpiresAt;

        public string? PhotoPath { get; set; }
    }
}
