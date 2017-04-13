using System;
using System.Threading.Tasks;
using Android.OS;
using Android.Views;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms.Android;
using Fragment = Android.Support.V4.App.Fragment;
using Android.Content;
using FinalYearProject.Mobile.Activities;
using FinalYearProject.Mobile.Services;
using Android.Util;
using Android.App;

namespace FinalYearProject.Mobile.Fragments
{
    public class BarcodeScanFragment : Fragment
    {
        MobileBarcodeScanner _scanner;
        private ProgressDialog _mProgressDialog;
        Intent _nextActivity;

        public override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Activity.Title = "Barcode Scan";
            DoLookup("test");
            //try
            //{
            //    await Scanny();
            //}
            //catch (Exception ex)
            //{
            //    Log.Debug("Scanning Error", ex.ToString());
            //}
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
            MobileBarcodeScanner.Initialize(Activity.Application);
            Platform.Init();
            _scanner = new MobileBarcodeScanner();

            _scanner.UseCustomOverlay = false;

            _scanner.TopText = "Hold the camera up to the barcode\nAbout 6 inches away";
            _scanner.BottomText = "Wait for the barcode to automatically scan!";

            ShowProgressDialog();

            //Start scanning
            var result = await _scanner.Scan();

            if(result != null && !string.IsNullOrEmpty(result.Text))
            {
                DoLookup(result.Text);
            }
            else
            {
                Activity.SupportFragmentManager.PopBackStack();
            }
        }

        private async void DoLookup(string text)
        {
            IAPIService restService = new APIService();

            var productString = await restService.SearchByEAN(text);
            //HideProgressDialog();

            var fragment = StoreListFragment.NewInstance();
            var myActivity = (MainActivity)this.Activity;
            myActivity.SetProduct(productString);

            fragment.Arguments.PutString("product", productString);

            Activity.SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, fragment)
                .AddToBackStack(null)
                .Commit();

            //_nextActivity = new Intent(this.Activity, typeof(StoreListFragment));
            //_nextActivity.PutExtra("product", productString);
            //_nextActivity.PutExtra("ean", text);
            //StartActivity(_nextActivity);
        }

        private void ShowProgressDialog()
        {
            if (_mProgressDialog == null)
            {
                _mProgressDialog = new ProgressDialog(this.Activity);
                _mProgressDialog.SetMessage(GetString(Resource.String.loading));
                _mProgressDialog.Indeterminate = true;
            }

            _mProgressDialog.Show();
        }

        private void HideProgressDialog()
        {
            if (_mProgressDialog != null && _mProgressDialog.IsShowing)
            {
                _mProgressDialog.Hide();
            }
        }

        //public override void OnDestroy()
        //{
        //    base.OnDestroy();
        //    _scanner.Cancel();
        //}
        //public override void OnPause()
        //{
        //    base.OnPause();
        //    _scanner.Cancel();
        //}
    }
}