using System;
using Android;
using Android.App;
using Android.Content.PM;
using Android.Gms.Auth.Api.SignIn;
using Android.OS;
using Android.Gms.Common.Apis;
using Android.Util;
using FinalYearProject.Mobile.Helpers;
using FinalYearProject.Domain;
using FinalYearProject.Mobile.Fragments;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using FinalYearProject.Mobile.Adapters;
using Android.Views;
using System.Collections.Generic;
using FinalYearProject.Mobile.Services;
using Newtonsoft.Json.Linq;

namespace FinalYearProject.Mobile.Activities
{
    [Activity(Label = "Home", LaunchMode = LaunchMode.SingleTop, Icon = "@drawable/Icon")]
    public class MainActivity : BaseActivity, GoogleApiClient.IOnConnectionFailedListener
    {
        GoogleSignInAccount _gsc;
        List<OnlineStore> _onlineStores;

        private TabLayout tabLayout;
        private ViewPager viewPager;

        private int[] tabIcons = {
            Resource.Drawable.ic_search_white_24dp,
            Resource.Drawable.ic_network_wifi_white_24dp,
            Resource.Drawable.ic_store_white_24dp
        };

        private SearchResultFragment _searchResultsFragment;
        private OnlineStoresFragment _onlineStoresFragment;
        private LocalStoresFragment _offlineStoresFragment;
        private const string TAG = "MainActivity";

        private List<Guid> _onlineImpressionsGuid;
        private List<Guid> _localImpressionsGuid;

        readonly string[] Permissions =
            {
                Manifest.Permission.AccessCoarseLocation,
                Manifest.Permission.AccessFineLocation,
                Manifest.Permission.Camera
            };

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.main;
            }
        }

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(LayoutResource);
            base.OnCreateDrawer(savedInstanceState);

            _gsc = ((MainApplication)Application).GSC;

            GoogleSignInAccount _acct = ((MainApplication)this.Application).GSC;

            string id = _acct.Id;
            IAPIService serv = new APIService();
            var ans = await serv.CheckUser(id);
            if (ans == "-1" || ans == "null" || ans == null)
            {
                dynamic user = new JObject();
                user.email = _acct.Email;
                user.id = _acct.Id;
                user.name = _acct.DisplayName;
                await serv.AddUser(user);
            }
        }

        private async void GetLocation()
        {
            if (LocationHelper.Position == null)
            {
                try
                {
                    await LocationHelper.GetLocation();
                }
                catch (Exception ex)
                {
                    Log.Debug(TAG, ex.ToString());
                }
            }
        }

        public override void OnBackPressed()
        {
            if (SupportFragmentManager.BackStackEntryCount > 0)
            {
                SupportFragmentManager.PopBackStack();
            }
            else
            {
                base.OnBackPressed();
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            GetLocation();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
        
        public void SetOnlineStores(List<OnlineStore> onlineStoresString)
        {
            _onlineStores = onlineStoresString;
        }

        public List<OnlineStore> GetOnlineStores()
        {
            return _onlineStores;
        }

        public void SetLocalImpressionsGuid(List<Guid> localImpressionsGuid)
        {
            _localImpressionsGuid = localImpressionsGuid;
        }

        public void SetOnlineImpressionsGuid(List<Guid> onlineImpressionsGuid)
        {
            _onlineImpressionsGuid = onlineImpressionsGuid;
        }

        public Guid GetLocalImpressionGuid(int position)
        {
            return _localImpressionsGuid[position];
        }

        public Guid GetOnlineImpressionGuid(int position)
        {
            return _onlineImpressionsGuid[position];
        }

        internal void SetupResultsView()
        {
            View _pListingsLayout = this.LayoutInflater.Inflate(Resource.Layout.main, null);
            viewPager = _pListingsLayout.FindViewById<ViewPager>(Resource.Id.viewpager);
            setupViewPager(viewPager);

            tabLayout = _pListingsLayout.FindViewById<TabLayout>(Resource.Id.sliding_tabs);
            tabLayout.SetupWithViewPager(viewPager);
            setupTabIcons();
        }

        private void setupTabIcons()
        {
            tabLayout.GetTabAt(0).SetIcon(tabIcons[0]);
            tabLayout.GetTabAt(1).SetIcon(tabIcons[1]);
            tabLayout.GetTabAt(2).SetIcon(tabIcons[2]);
        }

        private void InitialiseFragments()
        {
            _searchResultsFragment = new SearchResultFragment();
            _onlineStoresFragment = new OnlineStoresFragment();
            _offlineStoresFragment = new LocalStoresFragment();
        }

        public void setupViewPager(ViewPager viewPager)
        {
            InitialiseFragments();
            ViewPagerAdapter _adapter = new ViewPagerAdapter(SupportFragmentManager);
            _adapter.addFragment(_searchResultsFragment, "Search Results");
            _adapter.addFragment(_onlineStoresFragment, "Online");
            _adapter.addFragment(_offlineStoresFragment, "Offline");

            viewPager.Adapter = _adapter;
        }
        
    }
}