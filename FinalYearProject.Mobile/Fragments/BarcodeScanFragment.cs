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
using Android.Gms.Auth.Api.SignIn;

namespace FinalYearProject.Mobile.Fragments
{
    public class BarcodeScanFragment : Fragment
    {
        MobileBarcodeScanner _scanner;
        private ProgressDialog _mProgressDialog;
        private GoogleSignInAccount _acct;

        public override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Activity.Title = "Barcode Scan";

            _acct = ((MainApplication)Activity.Application).GSC;

            await DoLookup("test");
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

        private async Task DoLookup(string text)
        {
            IAPIService apiService = new APIService();
            ShowProgressDialog();
            text = "555";
            var productString = await apiService.SearchByEAN(text);
            //HideProgressDialog();

            var intent = new Intent(Activity, typeof(ProductListingsActivity));
            intent.PutExtra("productString", productString);
            intent.PutExtra("account", _acct);
            StartActivity(intent);
            
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