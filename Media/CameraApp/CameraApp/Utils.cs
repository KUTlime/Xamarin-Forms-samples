using System.Threading.Tasks;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace CameraApp
{
    public static class Utils
    {
        public static string MyAppLabel = "CameraApp";

        public static async Task<bool> CheckPermissions(Permission permission)
        {
            var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
            bool request = false;
            if (permissionStatus == PermissionStatus.Denied)
            {
                if (Device.RuntimePlatform == Device.iOS)
                {

                    var title = $"{permission} Permission";
                    var question = $"To use this plugin the {permission} permission is required. Please go into Settings and turn on {permission} for the app.";
                    var positive = "Settings";
                    var negative = "Maybe Later";
                    var task = Application.Current?.MainPage?.DisplayAlert(title, question, positive, negative);
                    if (task == null) return false;

                    var result = await task;
                    if (result) CrossPermissions.Current.OpenAppSettings();

                    return false;
                }

                request = true;

            }

            if (!request && permissionStatus == PermissionStatus.Granted) return true;
            {
                var newStatus = await CrossPermissions.Current.RequestPermissionsAsync(permission);
                if (!newStatus.ContainsKey(permission) || newStatus[permission] == PermissionStatus.Granted) return true;

                var title = $"{permission} Permission";
                var question = $"To use the plugin the {permission} permission is required.";
                var positive = "Settings";
                var negative = "Maybe Later";
                var task = Application.Current?.MainPage?.DisplayAlert(title, question, positive, negative);
                if (task == null) return false;

                var result = await task;
                if (result) CrossPermissions.Current.OpenAppSettings();

                return false;
            }

        }
    }
}
