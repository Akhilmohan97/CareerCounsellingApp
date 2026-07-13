using CareerCounsellingApp.PhotoCapture.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.PhotoCapture.Services
{
    public class NetworkService : INetworkService
    {
        public string BuildCaptureUrl(Guid sessionId)
        {
            return $"http://{GetLocalIPAddress()}:{GetAvailablePort()}/capture?session={sessionId}";
        }

        public int GetAvailablePort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);

            listener.Start();

            int port = ((IPEndPoint)listener.LocalEndpoint).Port;

            listener.Stop();

            return port;
        }

        public string GetLocalIPAddress()
        {
            foreach (var network in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (network.OperationalStatus != OperationalStatus.Up)
                    continue;
                if (network.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                    continue;
                var properties = network.GetIPProperties();
                foreach (var address in properties.UnicastAddresses)
                {
                    if (address.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return address.Address.ToString();
                    }
                }
            }
            throw new Exception("Unable to determine local IP.");
        }
    }
}
