using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Views;
using Android.Content;
using Plugin.Geolocator.Abstractions;
using Android;
using Android.Support.V7.App;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms.Android;
using Android.Content.PM;
using Plugin;
using Plugin.Permissions.Abstractions;
using Plugin.Permissions;
using Android.Gms.Auth.Api.SignIn;
using Newtonsoft.Json;
using Android.Runtime;

namespace FinalYearProject.Mobile.Activities
{
    [Preserve(AllMembers = true)]

    [Activity(Label = "FinalYearProject.Mobile", Icon = "@drawable/icon")]
    public class HomeActivity : AppCompatActivity
    {
        Button _searchButton;
        //Button _barcodeScannerButton;
        AutoCompleteTextView _autoTexView;

        Intent _nextActivity; 

        //MobileBarcodeScanner _scanner;

        const int RequestLocationId = 0;
        private const int MY_PERMISSIONS_REQUEST_CAMERA = 5;
        private const int MY_PERMISSIONS_REQUEST_LOCATION = 4;
        readonly string[] PermissionsLocation =
            {
                Manifest.Permission.AccessCoarseLocation,
                Manifest.Permission.AccessFineLocation
            };

        View layout;

        Position pos;
        protected override  void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            GoogleSignInAccount gsc = Intent.GetParcelableExtra("account") as GoogleSignInAccount;

            MobileBarcodeScanner.Initialize(Application);
            Platform.Init();

            //_scanner = new MobileBarcodeScanner();

            base.SetContentView(Resource.Layout.Home);

            layout = FindViewById(Resource.Layout.Home);
            //StartService(new Intent(this, typeof(Services.LocationService)));

            _autoTexView = (AutoCompleteTextView)FindViewById(Resource.Id.autoCompleteTextViewSearch);

            Button clearButton = (Button)FindViewById(Resource.Id.buttonClear);

            clearButton.Click += (object sender, EventArgs e) =>
            {
                _autoTexView.Text = "";
            };

            //_barcodeScannerButton = (Button)FindViewById(Resource.Id.buttonBarcodeScanner);
            //_barcodeScannerButton.Click += async (sender, ea) =>
            //{
            //    Android.Content.PM.Permission permissionCheck = CheckSelfPermission(Manifest.Permission.Camera);
            //    if (permissionCheck == Android.Content.PM.Permission.Denied)
            //    {
            //        if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            //        {
            //            RequestPermissions(new String[] { Manifest.Permission.Camera },
            //                MY_PERMISSIONS_REQUEST_CAMERA);
            //        }
            //        if (ShouldShowRequestPermissionRationale(Manifest.Permission.Camera))
            //        {
            //            Toast.MakeText(this, "Need the camera to scan barcodes!", ToastLength.Long).Show();
            //        }
            //    }
            //    else
            //    {
            //        Scanny();
            //    }

            //    var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Camera);

            //    if (status != PermissionStatus.Granted)
            //    {
            //        if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Plugin.Permissions.Abstractions.Permission.Camera))
            //        {
            //            //await DisplayAlert("Need location", "Gunna need that location", "OK");
            //        }

            //        var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Plugin.Permissions.Abstractions.Permission.Camera });
            //        status = results[Plugin.Permissions.Abstractions.Permission.Camera];
            //    }

            //    if (status == PermissionStatus.Granted)
            //    {
            //        Scanny();
            //    }
            //    else if (status != PermissionStatus.Unknown)
            //    {
            //        //await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
            //    }
            //};
            

            _searchButton = (Button)FindViewById(Resource.Id.buttonSearch);
            _searchButton.Click += (sender, e) =>
            {

                _nextActivity = new Intent(this, typeof(ProductListingsActivity));
                _nextActivity.PutExtra("location", pos.Latitude.ToString());
                StartActivity(_nextActivity);
            };
        }
        //void HandleScanResult(ZXing.Result result)
        //{
        //    string msg = "";

        //    if (result != null && !string.IsNullOrEmpty(result.Text))
        //        msg = "Found Barcode: " + result.Text;
        //    else
        //        msg = "Scanning Canceled!";

        //    RunOnUiThread(() => Toast.MakeText(this, msg, ToastLength.Long).Show());
        //}

        //public async void Scanny()
        //{
        //    _scanner.UseCustomOverlay = false;

        //    //We can customize the top and bottom text of the default overlay
        //    _scanner.TopText = "Hold the camera up to the barcode\nAbout 6 inches away";
        //    _scanner.BottomText = "Wait for the barcode to automatically scan!";

        //    //Start scanning
        //    var result = await _scanner.Scan();

        //    HandleScanResult(result);
        //}
        
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        //{
        //    PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //    PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //}



        //protected async void OnResume(Bundle bundle)
        //{
        //    var locator = CrossGeolocator.Current;
        //    locator.DesiredAccuracy = 50;

        //    if ((int)Build.VERSION.SdkInt < 23)
        //    {
        //        pos = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
        //    }
        //    else
        //    {
        //        const string permission = Manifest.Permission.AccessFineLocation;

        //        if (ContextCompat.CheckSelfPermission(this, permission) == Permission.Granted)
        //        {
        //            pos = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
        //        }
        //        else
        //        {
        //            if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.AccessFineLocation))
        //            {
        //                // Provide an additional rationale to the user if the permission was not granted
        //                // and the user would benefit from additional context for the use of the permission.
        //                // For example if the user has previously denied the permission.
        //                Log.Info("OK", "Displaying camera permission rationale to provide additional context.");

        //                Snackbar.Make(layout, "Location access is required to for data collection.", Snackbar.LengthIndefinite)
        //                    .SetAction("OK", v => RequestPermissions(PermissionsLocation, RequestLocationId))
        //                    .Show();
        //            }
        //            else
        //            {
        //                // Camera permission has not been granted yet. Request it directly.
        //                IOnRequestPermissionsResultCallback(PermissionsLocation, RequestLocationId);
        //            }
        //        }
        //    }
        //}
        //public async Task GetLocationCompatAsync()
        //{
        //    const string permission = Manifest.Permission.AccessFineLocation;

        //    if (ContextCompat.CheckSelfPermission(this, permission) == Permission.Granted)
        //    {
        //        await GetLocationAsync();
        //        return;
        //    }

        //    if (ActivityCompat.ShouldShowRequestPermissionRationale(this, permission))
        //    {
        //        //Explain to the user why we need to read the contacts
        //        Snackbar.Make(layout, "Location access is required to show coffee shops nearby.",
        //            Snackbar.LengthIndefinite)
        //            .SetAction("OK", v => RequestPermissions(PermissionsLocation, RequestLocationId))
        //            .Show();
        //        return;
        //    }

        //    RequestPermissions(PermissionsLocation, RequestLocationId);
        //}

        //async Task GetLocationAsync()
        //{
        //    try
        //    {
        //        var locator = CrossGeolocator.Current;
        //        locator.DesiredAccuracy = 100;
        //        await locator.GetPositionAsync(20000);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        //{
        //    switch (requestCode)
        //    {
        //        case RequestLocationId:
        //            {
        //                if (grantResults[0] == (int)Permission.Granted)
        //                {
        //                    //Permission granted
        //                }
        //                else
        //                {
        //                    //Permission Denied :(
        //                    //Disabling location functionality
        //                    var snack = Snackbar.Make(layout, "Location permission is denied.", Snackbar.LengthShort);
        //                    snack.Show();
        //                }
        //            }
        //            break;
        //    }
        //}

        //async Task GetLocationPermissionAsync()
        //{
        //    //Check to see if any permission in our group is available, if one, then all are
        //    const string permission = Manifest.Permission.AccessFineLocation;
        //    if (CheckSelfPermission(permission) == (int)Permission.Granted)
        //    {
        //        await GetLocationAsync();
        //        return;
        //    }

        //    //need to request permission
        //    if (ShouldShowRequestPermissionRationale(permission))
        //    {
        //        //Explain to the user why we need to read the contacts
        //        Snackbar.Make(layout, "Location access is required to for data collection.", Snackbar.LengthIndefinite)
        //                .SetAction("OK", v => RequestPermissions(PermissionsLocation, RequestLocationId))
        //                .Show();
        //        return;
        //    }
        //    //Finally request permissions with the list of permissions and Id
        //    RequestPermissions(PermissionsLocation, RequestLocationId);
        //}

        //protected override void OnResume()
        //{
        //    base.OnResume();
        //    string Provider = LocationManager.GpsProvider;

        //    if (locMgr.IsProviderEnabled(Provider))
        //    {
        //        locMgr.RequestLocationUpdates(Provider, 2000, 1, this);
        //    }
        //    else
        //    {
        //        //Log.Info(tag, Provider + " is not available. Does the device have location services enabled?");
        //    }
        //}

        //get result of query then pass to paroductListingActivity

        //int permissionCheck = Icont.checkSelfPermission(this, Manifest.Permission.AccessCoarseLocation);

        //locMgr = GetSystemService(Context.LocationService) as LocationManager;

        //Criteria locationCriteria = new Criteria();

        //locationCriteria.Accuracy = Accuracy.Coarse;
        //locationCriteria.PowerRequirement = Power.Medium;

        //string locationProvider = locMgr.GetBestProvider(locationCriteria, false);

        //if (locationProvider != null)
        //{
        //    locMgr.GetLastKnownLocation(locationProvider);
        //}
        //else
        //{
        //    //Log.Info(tag, "No location providers available");
        //}


    }
}

