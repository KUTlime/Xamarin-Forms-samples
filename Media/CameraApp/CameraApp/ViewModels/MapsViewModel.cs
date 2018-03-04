using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace CameraApp.ViewModels
{
    public class MapsViewModel : BaseViewModel
    {
        public Pin Pin { get; set; }

        public MapsViewModel()
        {
            Title = "MapsViewModel";
            Pin = new Pin
            {
                Position = new Position(Helpers.Settings.Longitude, Helpers.Settings.Latitude)
            };
        }
    }
}
