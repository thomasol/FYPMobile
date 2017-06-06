using System;
using System.Threading.Tasks;
using Android.OS;
using Android.Views;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms.Android;
using Fragment = Android.Support.V4.App.Fragment;
using FinalYearProject.Mobile.Services;
using Android.App;
using Android.Gms.Auth.Api.SignIn;
using Newtonsoft.Json.Linq;
using FinalYearProject.Domain;
using FinalYearProject.Mobile.Activities;
using Android.Util;

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
                await DoLookup(result.Text);
            }
            else
            {
                Activity.SupportFragmentManager.PopBackStack();
            }
        }

        private async Task DoLookup(string text)
        {
            IAPIService apiService = new APIService();
            text = "555";
            ShowProgressDialog();

            var productString = await apiService.SearchByEAN(text);

            SetProduct(productString);
            
            Fragment frag = ProductListingsFragment.NewInstance();
            HideProgressDialog();
            
            Activity.SupportFragmentManager.BeginTransaction()
            .Replace(Resource.Id.content_frame, frag)
            .AddToBackStack(null)
            .Commit();
        }

        private void SetProduct(string productString)
        {
            try
            {
                if (productString != "Data not available")
                {
                    JObject jsonResponse = JObject.Parse(productString);
                    Product _p = jsonResponse.ToObject<Product>();
                    var myActivity = (MainActivity)Activity;
                    myActivity.SetProduct(_p);
                }

            }
            catch (Exception ex)
            {
                Log.Debug("ProductString initialisation Fail", ex.ToString());
            }
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
        
    }
}