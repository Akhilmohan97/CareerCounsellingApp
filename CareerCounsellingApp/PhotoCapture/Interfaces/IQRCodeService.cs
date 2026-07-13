using System;
using System.Collections.Generic;
using Avalonia.Media.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.PhotoCapture.Interfaces
{
    public interface IQRCodeService
    {
        Bitmap Generate(string text);
    }
}
