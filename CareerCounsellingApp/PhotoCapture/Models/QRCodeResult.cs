using System;
using System.Collections.Generic;
using Avalonia.Media.Imaging;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.PhotoCapture.Models;

public sealed class QRCodeResult
{
    public required Bitmap Image { get; init; }

    public required string Url { get; init; }

    public required Guid SessionId { get; init; }
}
