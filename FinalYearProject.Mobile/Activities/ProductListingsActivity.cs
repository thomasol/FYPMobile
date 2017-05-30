using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FinalYearProject.Mobile.Helpers;
using FinalYearProject.Mobile.Services;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using FinalYearProject.Mobile.Adapters;
using FinalYearProject.Mobile.Fragments;
using Android.Support.Design.Widget;
using FinalYearProject.Domain;
using Android.Util;
using Android.Gms.Auth.Api.SignIn;
using Android.Support.V4.Widget;
using Android.Gms.Common.Apis;
using Android.Gms.Auth.Api;
using Android.Gms.Common;
using Android.Support.V4.View;

namespace FinalYearProject.Mobile.Activities
{
    [Activity(Label = "Search Results")]
    public class ProductListingsActivity :  BaseActivity, GoogleApiClient.IOnConnectionFailedListener
    {
        Android.Support.V4.App.Fragment fragment = null;
        GoogleSignInAccount _gsc;

        DrawerLayout _drawerLayout;
        NavigationView _navigationView;

        private TextView _mUsernameTextView;
        GoogleApiClient _mGoogleApiClient;
        int _oldPosition = -1;

        private TabLayout tabLayout;
        private ViewPager viewPager;

        private int[] tabIcons = {
            Resource.Drawable.ic_search_white_24dp,
            Resource.Drawable.ic_network_wifi_white_24dp,
            Resource.Drawable.ic_store_white_24dp
        };

        private SearchResultFragment _searchResultsFragment;
        private OnlineStoresFragment _onlineStoresFragment;
        private OfflineStoresFragment _offlineStoresFragment;

        Android.Support.V7.Widget.ShareActionProvider _actionProvider;
        Product _p;
        private const string TAG = "ProductListingsActivity";

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

            viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            setupViewPager(viewPager);

            tabLayout = FindViewById<TabLayout>(Resource.Id.sliding_tabs);
            tabLayout.SetupWithViewPager(viewPager);
            setupTabIcons();

            #region nav/drawer layout
            _drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            //Set hamburger items menu
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);
            //setup navigation view
            _navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            //handle navigation
            _navigationView.NavigationItemSelected += (sender, e) =>
            {
                e.MenuItem.SetChecked(true);

                switch (e.MenuItem.ItemId)
                {
                    //case Resource.Id.navSearch:
                    //    ListItemClicked(0);
                    //    break;
                    case Resource.Id.navBarcodeSearch:
                        ListItemClicked(1);
                        break;
                    case Resource.Id.navAccount:
                        ListItemClicked(2);
                        break;
                    case Resource.Id.navSignOut:
                        ListItemClicked(3);
                        break;
                    case Resource.Id.storeListTest:
                        ListItemClicked(4);
                        break;
                }
                //Snackbar.Make(_drawerLayout, "You selected: " + e.MenuItem.TitleFormatted, Snackbar.LengthLong)
                //    .Show();
                _drawerLayout.CloseDrawers();
            };

            _gsc = ((MainApplication)Application).GSC;

            //GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
            //       .RequestEmail()
            //       .Build();

            //_mGoogleApiClient = new GoogleApiClient.Builder(this)
            //        .EnableAutoManage(this, this)
            //        .AddApi(Auth.GOOGLE_SIGN_IN_API, gso)
            //        .Build();
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

        public void SetProduct(Product productString)
        {
            _p = productString;
        }

        public Product GetProduct()
        {
            return _p;
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
            _offlineStoresFragment = new OfflineStoresFragment();
        }

        public void setupViewPager(ViewPager viewPager)
        {
            InitialiseFragments();
            ViewPagerAdapter adapter = new ViewPagerAdapter(SupportFragmentManager);
            adapter.addFragment(_searchResultsFragment, "Search Results");
            adapter.addFragment(_onlineStoresFragment, "Online");
            adapter.addFragment(_offlineStoresFragment, "Offline");

            viewPager.Adapter = adapter;
        }

        private async Task CreateImpressionEvent()
        {
            try
            {
                IAPIService restService = new APIService();
                dynamic impressionEvent = new JObject();
                var pos = LocationHelper.Position;

                var searchGUID = Guid.NewGuid();
                impressionEvent.SearchGUID = searchGUID;
                impressionEvent.CreatedAt = DateTime.Now;
                impressionEvent.Lon = pos.Longitude;
                impressionEvent.Lat = pos.Latitude;
                impressionEvent.EAN = _p.Ean;
                impressionEvent.Type = 1;

                for (int i = 0; i < _p.LocalStores.Count; i++)
                {
                    var impressionGUID = Guid.NewGuid();
                    impressionEvent.Description = "Search Event Local Store";
                    impressionEvent.GUID = impressionGUID;
                    impressionEvent.StoreCode = _p.LocalStores[i].StoreCode;
                    await restService.SaveEvent(impressionEvent);
                }
                for (int i = 0; i < _p.OnlineStores.Count; i++)
                {
                    var impressionGUID = Guid.NewGuid();
                    impressionEvent.Description = "Search Event Online Store";
                    impressionEvent.GUID = impressionGUID;
                    impressionEvent.Url = _p.OnlineStores[i].Url;
                    await restService.SaveEvent(impressionEvent);
                }
            }
            catch (Exception ex)
            {
                Log.Debug(TAG, ex.ToString());
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.menu_share1, menu);

            var shareItem = menu.FindItem(Resource.Id.action_share);
            var provider = MenuItemCompat.GetActionProvider(shareItem);

            _actionProvider = provider.JavaCast<Android.Support.V7.Widget.ShareActionProvider>();

            _mUsernameTextView = (TextView)FindViewById(Resource.Id.usernameHeader);
            _mUsernameTextView.Text = _gsc.DisplayName + " signed in.";

            return base.OnCreateOptionsMenu(menu);
        }

        private async void ListItemClicked(int position)
        {
            //this way we don't load twice, but you might want to modify this a bit.
            if (position == _oldPosition)
                return;
            _oldPosition = position;

            switch (position)
            {
                case 0:
                    
                    fragment = SearchFragment.NewInstance();
                    break;
                case 1:
                    fragment = BarcodeScanFragment.NewInstance();
                    break;
                case 2:
                    _searchResultsFragment.Dispose();
                    break;
                case 3:
                    SignOut();
                    var result = await Auth.GoogleSignInApi.SignOut(_mGoogleApiClient);
                    if (result.Status.IsSuccess)
                    {
                        Intent nextActivity = new Intent(this, typeof(SignInActivity));
                        nextActivity.AddFlags(ActivityFlags.ClearTop | ActivityFlags.ClearTask | ActivityFlags.NewTask);
                        StartActivity(nextActivity);
                        Finish();
                        return;
                    }
                    break;
                case 4:
                    fragment = BarcodeScanFragment.NewInstance();
                    //var _nextActivity = new Intent(this, typeof(ProductListingsActivity));
                    //StartActivity(_nextActivity);
                    break;
            }

            SupportFragmentManager.BeginTransaction()
            .Replace(Resource.Id.content_frame, fragment)
            .AddToBackStack(null)
            .Commit();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    _drawerLayout.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            Log.Debug(TAG, "Main Activity Connection Error");
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        private void SignOut()
        {
            GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                   .RequestEmail()
                   .Build();

            _mGoogleApiClient = new GoogleApiClient.Builder(this)
                    .EnableAutoManage(this, this)
                    .AddApi(Auth.GOOGLE_SIGN_IN_API, gso)
                    .Build();
        }
    }
}