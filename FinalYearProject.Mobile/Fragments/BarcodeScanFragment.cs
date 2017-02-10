using System.Threading.Tasks;
using Android.OS;
using Android.Views;
using Android.Widget;
using FinalYearProject.Mobile.Helpers;
using Plugin.Geolocator.Abstractions;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms.Android;
using Fragment = Android.Support.V4.App.Fragment;

namespace FinalYearProject.Mobile.Fragments
{
    public class BarcodeScanFragment : Fragment
    {
        MobileBarcodeScanner _scanner;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            MobileBarcodeScanner.Initialize(Activity.Application);
            Platform.Init();
            _scanner = new MobileBarcodeScanner();
            Scanny();
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

        private async void Scanny()
        {
            _scanner.UseCustomOverlay = false;

            //We can customize the top and bottom text of the default overlay
            _scanner.TopText = "Hold the camera up to the barcode\nAbout 6 inches away";
            _scanner.BottomText = "Wait for the barcode to automatically scan!";

            //Start scanning
            var result = await _scanner.Scan();

            HandleScanResult(result);
        }

        void HandleScanResult(ZXing.Result result)
        {
            string msg = "";

            if (result != null && !string.IsNullOrEmpty(result.Text))
                msg = "Found Barcode: " + result.Text;
            //go search db
            //if exists display results
            //else inform not in db
            else
                msg = "Scanning Canceled!";

            Activity.RunOnUiThread(() => Toast.MakeText(Context, msg, ToastLength.Long).Show());
        }

    }
}