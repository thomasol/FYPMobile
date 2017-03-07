using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using FinalYearProject.Mobile.Fragments;
using Android.Gms.Common.Apis;
using Android.Gms.Auth.Api;
using Android.Gms.Location;
using Android.Locations;
using Android.Renderscripts;
using Android.Util;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions;
using Fragment = Android.Support.V4.App.Fragment;
using FinalYearProject.Mobile.Helpers;

namespace FinalYearProject.Mobile.Activities
{
    [Activity(Label = "Home", LaunchMode = LaunchMode.SingleTop, Icon = "@drawable/Icon")]
    public class MainActivity : BaseActivity, GoogleApiClient.IOnConnectionFailedListener
    {
        GoogleSignInAccount _gsc;

        DrawerLayout _drawerLayout;
        NavigationView _navigationView;

        private TextView _mUsernameTextView;
        GoogleApiClient _mGoogleApiClient;

        private TextView _mLatitudeText;
        private TextView _mLongitudeText;

        private Location _mLastLocation;
        private LocationRequest locRequest;
        private const string TAG = "MainActivity";

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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
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
                    case Resource.Id.navSearch:
                        ListItemClicked(0);
                        break;
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

                Snackbar.Make(_drawerLayout, "You selected: " + e.MenuItem.TitleFormatted, Snackbar.LengthLong)
                    .Show();

                _drawerLayout.CloseDrawers();
            };


            //if first time you will want to go ahead and click first item.
            if (savedInstanceState == null)
            {
                ListItemClicked(0);
            }
            //if (LocationHelper.Position == null)
            //{
            //    try
            //    {
            //        LocationHelper.GetLocation();
            //    }
            //    catch (Exception ex)
            //    {
            //        Log.Debug(TAG, ex.ToString());
            //    }
            //}

            _gsc = Intent.GetParcelableExtra("account") as GoogleSignInAccount;
            GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                   .RequestEmail()
                   .Build();

            _mGoogleApiClient = new GoogleApiClient.Builder(this)
                    .EnableAutoManage(this /* FragmentActivity */, this /* OnConnectionFailedListener */)
                    .AddApi(Auth.GOOGLE_SIGN_IN_API, gso)
                    .Build();
        }

        Android.Support.V7.Widget.ShareActionProvider _actionProvider;

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            //change menu_share1 to your name
            this.MenuInflater.Inflate(Resource.Menu.menu_share1, menu);

            var shareItem = menu.FindItem(Resource.Id.action_share);
            var provider = MenuItemCompat.GetActionProvider(shareItem);

            _actionProvider = provider.JavaCast<Android.Support.V7.Widget.ShareActionProvider>();

            var intent = new Intent(Intent.ActionSend);
            intent.SetType("text/plain");
            intent.PutExtra(Intent.ExtraText, "Time to share some text!");

            _actionProvider.SetShareIntent(intent);

            _mUsernameTextView = (TextView)FindViewById(Resource.Id.usernameHeader);
            _mUsernameTextView.Text = _gsc.DisplayName + " Signed in.";

            return base.OnCreateOptionsMenu(menu);
        }

        int _oldPosition = -1;
        private async void ListItemClicked(int position)
        {
            //this way we don't load twice, but you might want to modify this a bit.
            if (position == _oldPosition)
                return;

            _oldPosition = position;

            Fragment fragment = null;
            switch (position)
            {
                case 0:
                    fragment = SearchFragment.NewInstance();
                    break;
                case 1:
                    fragment = BarcodeScanFragment.NewInstance();
                    break;
                case 2:
                    fragment = AccountFragment.NewInstance();
                    Bundle bundle = new Bundle();
                    bundle.PutParcelable("account", _gsc);
                    fragment.Arguments = bundle;
                    break;
                case 3:
                    var result = await Auth.GoogleSignInApi.SignOut(_mGoogleApiClient);
                    if (result.Status.IsSuccess)
                    {
                        Intent nextActivity = new Intent(this, typeof(SignInActivity));
                        nextActivity.AddFlags( ActivityFlags.ClearTop | ActivityFlags.ClearTask | ActivityFlags.NewTask);
                        StartActivity(nextActivity);
                        Finish();
                        return;
                    }
                    break;
                case 4:
                    fragment = StoreListFragment.NewInstance();
                    break;
            }

            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, fragment)
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
        
    }
}

