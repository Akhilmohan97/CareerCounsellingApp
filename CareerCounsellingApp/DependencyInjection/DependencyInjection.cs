using CareerCounsellingApp.PhotoCapture.Interfaces;
using CareerCounsellingApp.PhotoCapture.Services;
using CareerCounsellingApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceProvider Configure()
        {
            var service = new ServiceCollection();
            service.AddSingleton<IPhotoCaptureService,PhotoCaptureService>();
            service.AddSingleton<INetworkService, NetworkService>();
            service.AddSingleton<IQRCodeService,QRCodeService>();
            service.AddTransient<PhotoCaptureWindow>();
            service.AddTransient<PhotoCaptureViewModel>();
            return service.BuildServiceProvider();
        }
    }
}
