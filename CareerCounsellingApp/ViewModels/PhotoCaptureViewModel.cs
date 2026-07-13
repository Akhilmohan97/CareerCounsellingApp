using Avalonia.Media.Imaging;
using CareerCounsellingApp.Helpers;
using CareerCounsellingApp.PhotoCapture.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CareerCounsellingApp.ViewModels
{
    public class PhotoCaptureViewModel:INotifyPropertyChanged
    {
        public ICommand CancelCommand { get; set; }
        private Bitmap? qrCode;
        public PhotoCaptureViewModel()
        {
            CancelCommand = new RelayCommand(Cancel);
        }

        public void Cancel()
        {
            var qr = AppServices.Provider.GetRequiredService<IQRCodeService>();

            var bitmap = qr.Generate("Hello World");
            QRCode= bitmap;
        }
        public Bitmap? QRCode
        {
            get { return qrCode; }
            set { qrCode = value;
                OnPropertyChanged("QRCode");
            }
        }

        private string status= "Waiting for phone...";

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        private string countdown = "Waiting for phone...";

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(name));
        }

        public string Countdown
        {
            get { return countdown; }
            set { countdown = value; }
        }

    }
}
