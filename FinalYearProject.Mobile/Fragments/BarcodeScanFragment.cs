using System;
using System.Threading.Tasks;
using Android.OS;
using Android.Views;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms.Android;
using Fragment = Android.Support.V4.App.Fragment;
using FinalYearProject.Mobile.Services;
using Android.App;
using FinalYearProject.Domain;
using FinalYearProject.Mobile.Activities;
using Android.Util;
using System.Collections.Generic;
using System.Linq;

namespace FinalYearProject.Mobile.Fragments
{
    public class BarcodeScanFragment : Fragment
    {
        MobileBarcodeScanner _scanner;
        private ProgressDialog _mProgressDialog;

        public override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Activity.Title = "Barcode Scan";

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
            text = "05099206056213";
            ShowProgressDialog();

            var onlineStores = await apiService.SearchByEAN(text);
            var offlineStores = await apiService.SearchByEANOffline(text);
            offlineStores = offlineStores.Where(s => s.Address1 != null).ToList();
            onlineStores = onlineStores.Where(s => s.Url != null).ToList();
            SetOnlineStores(onlineStores);
            SetOfflineStores(offlineStores);

            Fragment frag = ProductListingFragment.NewInstance();
            HideProgressDialog();
            
            Activity.SupportFragmentManager.BeginTransaction()
            .Replace(Resource.Id.content_frame, frag)
            .AddToBackStack(null)
            .Commit();
        }

        private void SetOfflineStores(List<OfflineStore> offlineStores)
        {
            try
            {
                if (offlineStores != null)
                {
                    var myActivity = (MainActivity)Activity;
                    myActivity.SetOfflineStores(offlineStores);
                }

            }
            catch (Exception ex)
            {
                Log.Debug("Set OfflineStores Fail", ex.ToString());
            }
        }

        private void SetOnlineStores(List<OnlineStore> onlineStores)
        {
            try
            {
                if (onlineStores != null)
                {
                    var myActivity = (MainActivity)Activity;
                    myActivity.SetOnlineStores(onlineStores);
                }

            }
            catch (Exception ex)
            {
                Log.Debug("Set OnlineStores Fail", ex.ToString());
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