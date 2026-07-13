using Avalonia.Media.Imaging;
using CareerCounsellingApp.PhotoCapture.Interfaces;
using QRCoder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.PhotoCapture.Services
{
    internal class QRCodeService : IQRCodeService
    {
        public Bitmap Generate(string text)
        {
            using var generator = new QRCodeGenerator();

            using var data = generator.CreateQrCode(
                text,
                QRCodeGenerator.ECCLevel.Q);

            var png = new PngByteQRCode(data);

            byte[] bytes = png.GetGraphic(20);

            return new Bitmap(new MemoryStream(bytes));
        }
    }
}
