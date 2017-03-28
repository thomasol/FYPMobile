using System;
using System.Threading.Tasks;
using Android.OS;
using Android.Views;
using Android.Widget;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms.Android;
using Fragment = Android.Support.V4.App.Fragment;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Android.Content;
using FinalYearProject.Mobile.Activities;
using FinalYearProject.Mobile.Services;

namespace FinalYearProject.Mobile.Fragments
{
    public class BarcodeScanFragment : Fragment
    {
        MobileBarcodeScanner _scanner;

        string content;
        Intent _nextActivity;

        public override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Activity.Title = "Barcode Scan";
            MobileBarcodeScanner.Initialize(Activity.Application);
            Platform.Init();
            _scanner = new MobileBarcodeScanner();
            await Scanny();
        }

        public static BarcodeScanFragment NewInstance()
        {
            var frag1 = new BarcodeScanFragment { Arguments = new Bundle() };
            return frag1;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            return inflater.Inflate(Resource.Layout.barcodeScanFragment, null);
        }

        private async Task Scanny()
        {
            _scanner.UseCustomOverlay = false;

            //We can customize the top and bottom text of the default overlay
            _scanner.TopText = "Hold the camera up to the barcode\nAbout 6 inches away";
            _scanner.BottomText = "Wait for the barcode to automatically scan!";

            //Start scanning
            var result = await _scanner.Scan();
            //_scanner.Cancel();
            IRESTService restService = new RESTService();
     
            var productString = await restService.SearchByEAN(result.Text);
            //var t = await HandleScanResult(result);
            _nextActivity = new Intent(this.Activity, typeof(ProductListingsActivity));
            _nextActivity.PutExtra("product", productString);
            StartActivity(_nextActivity);
        }

        //async Task<string> HandleScanResult(ZXing.Result result)
        //{
        //    string msg = "";

        //    if (result != null && !string.IsNullOrEmpty(result.Text))
        //    {
        //        msg = "Found Barcode: " + result.Text;
        //        var x =  SearchByEAN(result.Text);
        //        return await x;
        //    }
        //    else
        //    {
        //        return "-1";
        //        msg = "Scanning Canceled!";
        //    }
        //    Activity.RunOnUiThread(() => Toast.MakeText(Context, msg, ToastLength.Long).Show());
        //}
    }
}