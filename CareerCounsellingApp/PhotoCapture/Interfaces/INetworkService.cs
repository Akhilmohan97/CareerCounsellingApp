using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.PhotoCapture.Interfaces
{
    public interface INetworkService
    {
        string GetLocalIPAddress();

        int GetAvailablePort();

        string BuildCaptureUrl(Guid sessionId);
    }
}
