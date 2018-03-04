using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace CameraApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Barcode : ContentPage
    {
        public Barcode()
        {
            InitializeComponent();
        }

        private void OpenScanner(object sender, EventArgs e)
        {
            Scanner();
        }

        private async void Scanner()
        {
            // New scanner page from ZXing forms
            var scannerPage = new ZXingScannerPage();

            scannerPage.OnScanResult += (result) =>
            {
                // Turn off realtime scanning.
                scannerPage.IsScanning = false;

                // Alert with scanned barcode
                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PopAsync();
                    DisplayAlert("Scanned code", result.Text, "OK");
                });
                LabelWithBarcode.Text = $"Scanned barcode: {result.Text}";
            };

            // Getting back from scanner
            await Navigation.PushAsync(scannerPage);

        }
    }
}