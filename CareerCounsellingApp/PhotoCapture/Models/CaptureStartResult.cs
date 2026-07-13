using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;

namespace CareerCounsellingApp.PhotoCapture.Models;

public sealed class CaptureStartResult
{
    public required Guid SessionId { get; init; }

    public required Bitmap QRCode { get; init; }

    public required string Url { get; init; }

    public required DateTime ExpiresAt { get; init; }
}
