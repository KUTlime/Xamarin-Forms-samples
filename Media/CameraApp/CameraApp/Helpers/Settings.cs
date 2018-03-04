using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions.Abstractions;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace CameraApp.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        #region Setting Constants

        private const string SettingsKey = "settings_key";
        private static readonly string SettingsDefault = string.Empty;

        public static string GeneralSettings
        {
            get => AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
            set => AppSettings?.AddOrUpdateValue(SettingsKey, value);
        }
        #endregion

        #region Setting Longitude

        private static string _longitude = "Longitude";

        private static readonly double DefaultLongitude = 0.0;

        public static double Longitude
        {
            get => AppSettings.GetValueOrDefault(_longitude, DefaultLongitude);
            set => AppSettings?.AddOrUpdateValue(_longitude, value);
        }

        #endregion

        #region Setting Latitude

        private static string _latitude = "Latitude";

        private static readonly double DefaultLatitude = 0.0;

        public static double Latitude
        {
            get => AppSettings.GetValueOrDefault(_latitude, DefaultLatitude);
            set => AppSettings?.AddOrUpdateValue(_latitude, value);
        }

        #endregion

        #region Setting Altitude

        private static string _altitude = "Altitude";

        private static readonly double DefaultAltitude = 0.0;

        public static double Altitude
        {
            get => AppSettings.GetValueOrDefault(_altitude, DefaultAltitude);
            set => AppSettings?.AddOrUpdateValue(_altitude, value);
        }

        #endregion
    }
}
