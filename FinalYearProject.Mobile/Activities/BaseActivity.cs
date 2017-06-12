using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Support.V4.View;
using Android.Widget;
using Android.Gms.Auth.Api.SignIn;
using Android.Runtime;
using FinalYearProject.Mobile.Fragments;
using Android.Gms.Auth.Api;
using Android.Gms.Common.Apis;
using Android.Gms.Common;
using Android.Util;
using Fragment = Android.Support.V4.App.Fragment;
using Android.Graphics;
using System.Net;
using static Android.Graphics.PorterDuff;
using FinalYearProject.Mobile.Services;
using Newtonsoft.Json.Linq;

namespace FinalYearProject.Mobile.Activities
{
    public abstract class BaseActivity : AppCompatActivity, GoogleApiClient.IOnConnectionFailedListener
    {
        public Android.Support.V7.Widget.Toolbar Toolbar
        {
            get;
            set;
        }

        DrawerLayout _drawerLayout;
        NavigationView _navigationView;
        int _oldPosition = -1;
        Fragment fragment = null;
        private TextView _mUsernameTextView;
        GoogleSignInAccount _gsc;
        GoogleApiClient _mGoogleApiClient;
        private const string TAG = "BaseActivity";
        private GoogleSignInAccount _acct;
        Android.Support.V7.Widget.ShareActionProvider _actionProvider;
        private Bitmap _bitmap;
        bool _doubleBackToExitPressedOnce = false;
        private const int TIME_DELAY = 2000;
        private static long back_pressed;

        protected void OnCreateDrawer(Bundle bundle)
        {
            SetContentView(LayoutResource);

            #region toolbar drawerlayout
            Toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            if (Toolbar != null)
            {
                SetSupportActionBar(Toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);
            }

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

            //if first time you will want to go ahead and click first item.
            if (bundle == null)
            {
                ListItemClicked(0);
            }
            #endregion

            if (_bitmap == null)
            {
                _acct = ((MainApplication)this.Application).GSC;

                var bit = GetImageBitmapFromUrl();

                if (bit != null)
                {
                    _bitmap = getRoundedCornerBitmap(bit);
                }
            }

            _mGoogleApiClient = new GoogleApiClient.Builder(this)
                    .EnableAutoManage(this /* FragmentActivity */, this /* OnConnectionFailedListener */)
                    .AddApi(Auth.GOOGLE_SIGN_IN_API)
                    .Build();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.menu_share1, menu);
            _gsc = ((MainApplication)Application).GSC;

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
                    fragment = AccountFragment.NewInstance();
                    break;
                case 3:
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

        protected abstract int LayoutResource
        {
            get;
        }

        protected int ActionBarIcon
        {
            set { Toolbar.SetNavigationIcon(value); }
        }

        private void SignOut()
        {
            GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                    .RequestEmail()
                    .RequestProfile()
                    .Build();
        }

        protected override void OnStart()
        {
            base.OnStart();
            _mGoogleApiClient.Connect();
        }

        public override void OnBackPressed()
        {
            if (_doubleBackToExitPressedOnce)
            {
                base.OnBackPressed();
                Java.Lang.JavaSystem.Exit(0);
                return;
            }

            this._doubleBackToExitPressedOnce = true;
            Toast.MakeText(this, "Press back again to exit", ToastLength.Short).Show();

            new Handler().PostDelayed(() =>
            {
                _doubleBackToExitPressedOnce = false;
            }, 3000);
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            Log.Debug(TAG, "Base Activity Connection Error");
        }
        
        private Bitmap GetImageBitmapFromUrl()
        {
            Android.Net.Uri imageUri = _acct.PhotoUrl;
            string imageLocation = "https:" + imageUri.SchemeSpecificPart;

            Bitmap imageBitmap = null;
            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(imageLocation);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }
            Bitmap bMap = getRoundedCornerBitmap(imageBitmap);
            return bMap;
        }

        public static Bitmap getRoundedCornerBitmap(Bitmap bitmap)
        {
            Bitmap output = Bitmap.CreateBitmap(bitmap.Width,
                bitmap.Height, Bitmap.Config.Argb8888);
            Canvas canvas = new Canvas(output);

            Paint paint = new Paint();
            Rect rect = new Rect(0, 0, bitmap.Width, bitmap.Height);
            RectF rectF = new RectF(rect);
            const float roundPx = 45;

            paint.AntiAlias = true;
            canvas.DrawARGB(0, 0, 0, 0);
            paint.Color = (Color.ParseColor("#BAB399"));
            canvas.DrawRoundRect(rectF, roundPx, roundPx, paint);

            paint.SetXfermode(new PorterDuffXfermode(Mode.SrcIn));
            canvas.DrawBitmap(bitmap, rect, rect, paint);

            return output;
        }

        public Bitmap GetBitmap()
        {
            return _bitmap;
        }
        
    }
}

