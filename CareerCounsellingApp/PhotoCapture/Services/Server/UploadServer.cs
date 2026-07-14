using CareerCounsellingApp.PhotoCapture.Interfaces;
using CareerCounsellingApp.PhotoCapture.Interfaces.Server;
using CareerCounsellingApp.Services.AI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.PhotoCapture.Services.Server
{
    public class UploadServer : IUploadServer
    {
        private readonly int _port;
        public int Port => _port;
        public UploadServer()
        {
            _port = GetAvailablePort();
        }
        public async Task StartAsync()
        {
            var webroot = Path.Combine(AppContext.BaseDirectory, "PhotoCapture", "Web");
           var _host= Host.CreateDefaultBuilder().ConfigureWebHostDefaults(o =>
            {
                o.UseKestrel(); o.UseUrls($"http://0.0.0.0:{_port}"); o.Configure(app =>
                {
                    app.UseDefaultFiles(new DefaultFilesOptions { FileProvider = new PhysicalFileProvider(webroot) });
                    app.UseStaticFiles(new StaticFileOptions { FileProvider = new PhysicalFileProvider(webroot) });
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapPhotoCaptureEndpoints();
                    });
                });
            }).Build();
            await _host.StartAsync(); 
        }
        public int GetAvailablePort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);

            listener.Start();

            int port = ((IPEndPoint)listener.LocalEndpoint).Port;

            listener.Stop();

            return port;
        }
        public Task StopAsync()
        {
            throw new NotImplementedException();
        }
    }
}
