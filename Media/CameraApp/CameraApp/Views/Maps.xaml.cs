using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using CameraApp.Helpers;

namespace CameraApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Maps : ContentPage
    {
        public Maps()
        {
            InitializeComponent();
            var position = new Position(Helpers.Settings.Longitude, Helpers.Settings.Latitude);
            /*var pin = new Pin
            {
                Type = PinType.Place,
                Position = position,
                Label = "Current position",
                Address = "To be added"
            };

            customMap.Pins.Add(pin);*/
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(1.0)));
        }
    }
}