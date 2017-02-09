//using System;
//using System.Threading.Tasks;
//using Android;
//using Android.App;
//using Android.Content.PM;
//using Android.OS;
//using Android.Support.Design.Widget;
//using Android.Support.V4.App;
//using Android.Support.V4.Content;
//using Android.Support.V7.App;
//using Android.Views;
//using Android.Widget;
//using Plugin.Geolocator;

//namespace FinalYearProject.Mobile.Helpers
//{
//    [Activity(Label = "Marshmallow Permission", MainLauncher = true, Icon = "@mipmap/icon")]
//    public class LocationHelper : AppCompat
//    {
//        const int RequestLocationId = 0;

//        readonly string[] PermissionsLocation =
//            {
//                Manifest.Permission.AccessCoarseLocation,
//                Manifest.Permission.AccessFineLocation
//            };

//        TextView textLocation;
//        Button buttonGetLocation, buttonGetLocationCompat;
//        View layout;

//        async Task TryGetLocationAsync()
//        {
//            if ((int)Build.VERSION.SdkInt < 23)
//            {
//                await GetLocationAsync();
//                return;
//            }

//            await GetLocationPermissionAsync();
//        }

//        async Task GetLocationAsync()
//        {

//            textLocation.Text = "Getting Location";
//            try
//            {
//                var locator = CrossGeolocator.Current;
//                locator.DesiredAccuracy = 100;
//                var position = await locator.GetPositionAsync(20000);

//                textLocation.Text = string.Format("Lat: {0}  Long: {1}", position.Latitude, position.Longitude);
//            }
//            catch (Exception ex)
//            {

//                textLocation.Text = "Unable to get location: " + ex.ToString();
//            }
//        }

//        async Task GetLocationPermissionAsync()
//        {
//            const string permission = Manifest.Permission.AccessFineLocation;

//            if (CheckSelfPermission(permission) == (int)Permission.Granted)
//            {
//                await GetLocationAsync();
//                return;
//            }

//            if (ShouldShowRequestPermissionRationale(permission))
//            {
//                //Explain to the user why we need to read the contacts
//                Snackbar.Make(layout, "Location access is required to show coffee shops nearby.",
//                    Snackbar.LengthIndefinite)
//                    .SetAction("OK", v => RequestPermissions(PermissionsLocation, RequestLocationId))
//                    .Show();

//                return;
//            }

//            RequestPermissions(PermissionsLocation, RequestLocationId);

//        }

//        public override async void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults1)
//        {
//            switch (requestCode)
//            {
//                case RequestLocationId:
//                    {
//                        if (grantResults1[0] == (int)Permission.Granted)
//                        {
//                            //Permission granted
//                            var snack = Snackbar.Make(layout, "Location permission is available, getting lat/long.",
//                                            Snackbar.LengthShort);
//                            snack.Show();

//                            await GetLocationAsync();
//                        }
//                        else
//                        {
//                            //Permission Denied :(
//                            //Disabling location functionality
//                            var snack = Snackbar.Make(layout, "Location permission is denied.", Snackbar.LengthShort);
//                            snack.Show();
//                        }
//                    }
//                    break;
//            }
//        }

//        async Task GetLocationCompatAsync()
//        {
//            const string permission = Manifest.Permission.AccessFineLocation;

//            if (ContextCompat.CheckSelfPermission(this, permission) == (int)Permission.Granted)
//            {
//                await GetLocationAsync();
//                return;
//            }

//            if (ActivityCompat.ShouldShowRequestPermissionRationale(this, permission))
//            {
//                //Explain to the user why we need to read the contacts
//                Snackbar.Make(layout, "Location access is required to show coffee shops nearby.",
//                    Snackbar.LengthIndefinite)
//                    .SetAction("OK", v => RequestPermissions(PermissionsLocation, RequestLocationId))
//                    .Show();

//                return;
//            }

//            RequestPermissions(PermissionsLocation, RequestLocationId);
//        }

//    }
//}