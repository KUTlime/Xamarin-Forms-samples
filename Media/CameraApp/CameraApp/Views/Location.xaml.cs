using System;
using System.Collections.ObjectModel;
using System.Linq;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CameraApp;

namespace CameraApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Location : ContentPage
    {
        private Int32 _count;
        private bool _tracking;
        private Position _savedPosition;
        private ObservableCollection<Position> Positions { get; } = new ObservableCollection<Position>();

        public Location()
        {
            InitializeComponent();
            ListViewPositions.ItemsSource = Positions;
        }


        private async void ButtonCached_Clicked(object sender, EventArgs e)
        {
            try
            {
                var hasPermission = await Utils.CheckPermissions(Permission.Location);
                if (!hasPermission)
                    return;

                ButtonCached.IsEnabled = false;

                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = DesiredAccuracy.Value;
                LabelCached.Text = "Getting GPS...";

                var position = await locator.GetLastKnownLocationAsync();

                if (position == null)
                {
                    LabelCached.Text = "null cached location :(";
                    return;
                }

                _savedPosition = position;
                ButtonAddressForPosition.IsEnabled = true;
                LabelCached.Text =
                    $"Time: {position.Timestamp} \nLat: {position.Latitude} \nLong: {position.Longitude} \nAltitude: {position.Altitude} \nAltitude Accuracy: {position.AltitudeAccuracy} \nAccuracy: {position.Accuracy} \nHeading: {position.Heading} \nSpeed: {position.Speed}";

            }
            catch (Exception ex)
            {
                Android.Util.Log.Error(Utils.MyAppLabel, ex.ToString());
                await DisplayAlert("Uh oh", "Something went wrong, but don't worry we captured for analysis! Thanks.", "OK");
            }
            finally
            {
                ButtonCached.IsEnabled = true;
            }
        }

        private async void ButtonGetGPS_Clicked(object sender, EventArgs e)
        {
            try
            {
                var hasPermission = await Utils.CheckPermissions(Permission.Location);
                if (!hasPermission) return;

                ButtonGetGPS.IsEnabled = false;

                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = DesiredAccuracy.Value;
                labelGPS.Text = "Getting GPS...";

                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(Timeout.Value), null, IncludeHeading.IsToggled);

                if (position == null)
                {
                    labelGPS.Text = "null GPS :(";
                    return;
                }
                _savedPosition = position;
                ButtonAddressForPosition.IsEnabled = true;
                labelGPS.Text =
                    $"Time: {position.Timestamp} \nLat: {position.Latitude} \nLong: {position.Longitude} \nAltitude: {position.Altitude} \nAltitude Accuracy: {position.AltitudeAccuracy} \nAccuracy: {position.Accuracy} \nHeading: {position.Heading} \nSpeed: {position.Speed}";

            }
            catch (Exception ex)
            {
                Android.Util.Log.Error(Utils.MyAppLabel, ex.ToString());
                await DisplayAlert("Uh oh", "Something went wrong, but don't worry we captured for analysis! Thanks.", "OK");
            }
            finally
            {
                ButtonGetGPS.IsEnabled = true;
            }
        }

        private async void ButtonAddressForPosition_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (_savedPosition == null) return;

                var hasPermission = await Utils.CheckPermissions(Permission.Location);
                if (!hasPermission) return;

                ButtonAddressForPosition.IsEnabled = false;

                var locator = CrossGeolocator.Current;

                var address = await locator.GetAddressesForPositionAsync(_savedPosition, "RJHqIE53Onrqons5CNOx~FrDr3XhjDTyEXEjng-CRoA~Aj69MhNManYUKxo6QcwZ0wmXBtyva0zwuHB04rFYAPf7qqGJ5cHb03RCDw1jIW8l");
                var enumerable = address as Address[] ?? address.ToArray();
                if (!enumerable.Any()) LabelAddress.Text = "Unable to find address";

                var a = enumerable.FirstOrDefault();
                LabelAddress.Text = $"Address: Thoroughfare = {a.Thoroughfare}\nLocality = {a.Locality}\nCountryCode = {a.CountryCode}\nCountryName = {a.CountryName}\nPostalCode = {a.PostalCode}\nSubLocality = {a.SubLocality}\nSubThoroughfare = {a.SubThoroughfare}";

            }
            catch (Exception ex)
            {
                Android.Util.Log.Error(Utils.MyAppLabel, ex.ToString());
                await DisplayAlert("Uh oh", "Something went wrong, but don't worry we captured for analysis! Thanks.", "OK");
            }
            finally
            {
                ButtonAddressForPosition.IsEnabled = true;
            }
        }

        private async void ButtonTrack_Clicked(object sender, EventArgs e)
        {
            try
            {
                var hasPermission = await Utils.CheckPermissions(Permission.Location);
                if (!hasPermission)
                    return;

                if (_tracking)
                {
                    CrossGeolocator.Current.PositionChanged -= CrossGeolocator_Current_PositionChanged;
                    CrossGeolocator.Current.PositionError -= CrossGeolocator_Current_PositionError;
                }
                else
                {
                    CrossGeolocator.Current.PositionChanged += CrossGeolocator_Current_PositionChanged;
                    CrossGeolocator.Current.PositionError += CrossGeolocator_Current_PositionError;
                }

                if (CrossGeolocator.Current.IsListening)
                {
                    await CrossGeolocator.Current.StopListeningAsync();
                    labelGPSTrack.Text = "Stopped tracking";
                    ButtonTrack.Text = "Start Tracking";
                    _tracking = false;
                    _count = 0;
                }
                else
                {
                    Positions.Clear();
                    if (await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(TrackTimeout.Value), TrackDistance.Value,
                        TrackIncludeHeading.IsToggled, new ListenerSettings
                        {
                            ActivityType = (ActivityType)ActivityTypePicker.SelectedIndex,
                            AllowBackgroundUpdates = AllowBackgroundUpdates.IsToggled,
                            DeferLocationUpdates = DeferUpdates.IsToggled,
                            DeferralDistanceMeters = DeferalDistance.Value,
                            DeferralTime = TimeSpan.FromSeconds(DeferalTIme.Value),
                            ListenForSignificantChanges = ListenForSig.IsToggled,
                            PauseLocationUpdatesAutomatically = PauseLocation.IsToggled
                        }))
                    {
                        labelGPSTrack.Text = "Started tracking";
                        ButtonTrack.Text = "Stop Tracking";
                        _tracking = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error(Utils.MyAppLabel, ex.ToString());
                await DisplayAlert("Uh oh", "Something went wrong, but don't worry we captured for analysis! Thanks.", "OK");
            }
        }




        void CrossGeolocator_Current_PositionError(object sender, PositionErrorEventArgs e)
        {
            labelGPSTrack.Text = "Location error: " + e.Error.ToString();
        }

        void CrossGeolocator_Current_PositionChanged(object sender, PositionEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var position = e.Position;
                Positions.Add(position);
                _count++;
                LabelCount.Text = $"{_count} updates";
                labelGPSTrack.Text = string.Format("Time: {0} \nLat: {1} \nLong: {2} \nAltitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \nHeading: {6} \nSpeed: {7}",
                    position.Timestamp, position.Latitude, position.Longitude,
                    position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed);

            });
        }
    }
}