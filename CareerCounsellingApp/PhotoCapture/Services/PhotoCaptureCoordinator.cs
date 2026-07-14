using Avalonia.Controls;
using CareerCounsellingApp.PhotoCapture.Interfaces;
using CareerCounsellingApp.PhotoCapture.Interfaces.Server;
using CareerCounsellingApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.PhotoCapture.Services;

public class PhotoCaptureCoordinator : IPhotoCaptureCoordinator
{
private readonly IPhotoCaptureService _photoCaptureService;
private readonly INetworkService _networkService;
private readonly IQRCodeService _qrCodeService;
private readonly IServiceProvider _serviceProvider;
    private readonly IUploadServer _uploadServer;

    public PhotoCaptureCoordinator(
        IPhotoCaptureService photoCaptureService,
        INetworkService networkService,
        IQRCodeService qrCodeService,
        IServiceProvider serviceProvider,
        IUploadServer uploadServer)
    {
        _photoCaptureService = photoCaptureService;
        _networkService = networkService;
        _qrCodeService = qrCodeService;
        _serviceProvider = serviceProvider;
        _uploadServer=uploadServer;
    }
    public async Task ShowCaptureWindowAsync(Window owner)
    {
        await _uploadServer.StartAsync();
        var session = await _photoCaptureService.StartSessionAsync();
        var url = _networkService.BuildCaptureUrl(_uploadServer.Port,session.SessionId);
        var vm = _serviceProvider.GetRequiredService<PhotoCaptureViewModel>();
        var bitMap = _qrCodeService.Generate(url);
        var window = _serviceProvider.GetRequiredService<PhotoCaptureWindow>();
        vm.QRCode = bitMap;
        vm.Status = "Scan using your phone";
        vm.Countdown = "05:00";
        window.DataContext = vm;
        await window.ShowDialog(owner);
    }


}
