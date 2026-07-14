using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.PhotoCapture.Interfaces.Server
{
    public interface IUploadServer
    {
        int Port { get; }
        Task StartAsync();
        int GetAvailablePort();
        Task StopAsync();
    }
}
