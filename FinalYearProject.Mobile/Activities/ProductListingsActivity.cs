using System;
using Android.App;
using Android.Content;
using Android.OS;
using FinalYearProject.Mobile.Helpers;
using FinalYearProject.Mobile.Services;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using FinalYearProject.Mobile.Adapters;
using FinalYearProject.Mobile.Fragments;
using Android.Support.Design.Widget;
using FinalYearProject.Domain;
using Android.Util;
using Android.Gms.Common.Apis;
using Android.Support.V4.View;
using System.Collections.Generic;

namespace FinalYearProject.Mobile.Activities
{
    [Activity(Label = "Search Results")]
    public class ProductListingsActivity :  BaseActivity, GoogleApiClient.IOnConnectionFailedListener
    {
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

        Product _p;
        List<Guid> _localImpressionsGuid = null;
        List<Guid> _onlineImpressionsGuid = null;

        private const string TAG = "ProductListingsActivity";
        ViewPagerAdapter _adapter = null;
        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.ProductListings;
            }
        }
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(LayoutResource);
            base.OnCreateDrawer(savedInstanceState);

            viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            setupViewPager(viewPager);

            tabLayout = FindViewById<TabLayout>(Resource.Id.sliding_tabs);
            tabLayout.SetupWithViewPager(viewPager);
            setupTabIcons();

            #region nav/drawer layout
            //_drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ////Set hamburger items menu
            //SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);
            ////setup navigation view
            //_navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            ////handle navigation
            //_navigationView.NavigationItemSelected += (sender, e) =>
            //{
            //    e.MenuItem.SetChecked(true);

            //    switch (e.MenuItem.ItemId)
            //    {
            //        //case Resource.Id.navSearch:
            //        //    ListItemClicked(0);
            //        //    break;
            //        case Resource.Id.navBarcodeSearch:
            //            ListItemClicked(1);
            //            break;
            //        case Resource.Id.navAccount:
            //            ListItemClicked(2);
            //            break;
            //        case Resource.Id.navSignOut:
            //            ListItemClicked(3);
            //            break;
            //        case Resource.Id.storeListTest:
            //            ListItemClicked(4);
            //            break;
            //    }
            //    //Snackbar.Make(_drawerLayout, "You selected: " + e.MenuItem.TitleFormatted, Snackbar.LengthLong)
            //    //    .Show();
            //    _drawerLayout.CloseDrawers();
            //};

            //_gsc = ((MainApplication)Application).GSC;

            ////GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
            ////       .RequestEmail()
            ////       .Build();

            ////_mGoogleApiClient = new GoogleApiClient.Builder(this)
            ////        .EnableAutoManage(this, this)
            ////        .AddApi(Auth.GOOGLE_SIGN_IN_API, gso)
            ////        .Build();
            #endregion

            try
            {
                string productString = Intent.GetStringExtra("productString") ?? "Data not available";
                if (productString != "Data not available")
                {
                    JObject jsonResponse = JObject.Parse(productString);
                    _p = jsonResponse.ToObject<Product>();
                    SetProduct(_p);
                }

            }
            catch (Exception ex)
            {
                Log.Debug("ProductListings initialisation Fail", ex.ToString());
            }
            await CreateImpressionEvent();
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
            _adapter = new ViewPagerAdapter(SupportFragmentManager);
            _adapter.addFragment(_searchResultsFragment, "Search Results");
            _adapter.addFragment(_onlineStoresFragment, "Online");
            _adapter.addFragment(_offlineStoresFragment, "Offline");

            viewPager.Adapter = _adapter;
        }

        private async Task CreateImpressionEvent()
        {
            try
            {
                IAPIService restService = new APIService();
                dynamic impressionEvent = new JObject();
                var pos = LocationHelper.Position;

                var _searchGuid = Guid.NewGuid();
                impressionEvent.SearchGuid = _searchGuid;
                impressionEvent.CreatedAt = DateTime.Now;
                impressionEvent.Lon = pos.Longitude;
                impressionEvent.Lat = pos.Latitude;
                impressionEvent.EAN = _p.Ean;
                impressionEvent.Type = 1;

                for (int i = 0; i < _p.LocalStores.Count; i++)
                {
                    var impressionGuid = Guid.NewGuid();
                    impressionEvent.ImpressionGuid = impressionGuid;
                    impressionEvent.Description = "Search Event Local Store";
                    impressionEvent.StoreCode = _p.LocalStores[i].StoreCode;
                    _localImpressionsGuid[i] = impressionGuid;
                    await restService.SaveEvent(impressionEvent);
                }
                for (int i = 0; i < _p.OnlineStores.Count; i++)
                {
                    var impressionGuid = Guid.NewGuid();
                    impressionEvent.ImpressionGuid = impressionGuid;
                    impressionEvent.Description = "Search Event Online Store";
                    impressionEvent.Url = _p.OnlineStores[i].Url;
                    _onlineImpressionsGuid[i] = impressionGuid;
                    await restService.SaveEvent(impressionEvent);

                }
            }
            catch (Exception ex)
            {
                Log.Debug(TAG, ex.ToString());
            }
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        protected override void OnPause()
        {
            base.OnPause();
            Finish();
        }

        protected override void OnPostResume()
        {
            base.OnPostResume();
        }

        public Guid GetLocalImpressionGuid(int position)
        {
            return _localImpressionsGuid[position];
        }

        public Guid GetOnlineImpressionGuid(int position)
        {
            return _onlineImpressionsGuid[position];
        }
        
        public void SetProduct(Product productString)
        {
            _p = productString;
        }

        public Product GetProduct()
        {
            return _p;
        }
    }
}