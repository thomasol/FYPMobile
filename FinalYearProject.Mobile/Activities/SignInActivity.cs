using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common.Apis;
using Android.Gms.Common;
using Android.Gms.Auth.Api;
using Android.Support.V7.App;
using Android.Util;
using Android.Gms.Location;
using FinalYearProject.Mobile.Services;
using FinalYearProject.Mobile.Helpers;

namespace FinalYearProject.Mobile.Activities
{
    [Activity(Label = "FYP App", MainLauncher = true)]
    public class SignInActivity : AppCompatActivity, GoogleApiClient.IOnConnectionFailedListener, View.IOnClickListener
    {
        private const string TAG = "SignInActivity";
        private const int RC_SIGN_IN = 9001;

        private static GoogleApiClient _mGoogleApiClient;
        private TextView _mStatusTextView;
        private ProgressDialog _mProgressDialog;
        GoogleSignInAccount _acct;

        Intent _nextActivity;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            // Views
            _mStatusTextView = (TextView)FindViewById(Resource.Id.status);

            //// Button listeners
            FindViewById(Resource.Id.sign_in_button).SetOnClickListener(this);
            FindViewById(Resource.Id.sign_out_button).SetOnClickListener(this);
            FindViewById(Resource.Id.disconnect_button).SetOnClickListener(this);

            // [START configure_signin]
            // Configure sign-in to request the user's ID, email address, and basic
            // profile. ID and basic profile are included in DEFAULT_SIGN_IN.
            GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                    .RequestEmail()
                    .RequestProfile()
                    .Build();
            // [END configure_signin]

            // [START build_client]
            // Build a GoogleApiClient with access to the Google Sign-In API and the
            // options specified by gso.
            _mGoogleApiClient = new GoogleApiClient.Builder(this)
                    .EnableAutoManage(this /* FragmentActivity */, this /* OnConnectionFailedListener */)
                    .AddApi(Auth.GOOGLE_SIGN_IN_API, gso).AddApi(LocationServices.API)
                    .Build();
            // [END build_client]

            // [START customize_button]
            // Set the dimensions of the sign-in button.
            SignInButton signInButton = (SignInButton)FindViewById(Resource.Id.sign_in_button);
            // [END customize_button]
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            Log.Debug(TAG, "Connection Error!");
        }

        // [START onActivityResult]
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            // Result returned from launching the Intent from GoogleSignInApi.getSignInIntent(...);
            if (requestCode == RC_SIGN_IN)
            {
                GoogleSignInResult result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                HandleSignInResult(result);
            }
        }

        // [END onActivityResult]

        // [START handleSignInResult]
        private void HandleSignInResult(GoogleSignInResult result)
        {
            Log.Debug(TAG, "handleSignInResult:" + result.IsSuccess);
            if (result.IsSuccess)
            {
                // Signed in successfully, show authenticated UI.
                _acct = result.SignInAccount;
                //var id = _acct.Email;
                //IAPIService serv = new APIService();
                //var ans = await serv.CheckUser(_acct);
                _mStatusTextView.Text = Resource.String.signed_n_fmt + _acct.DisplayName;
                UpdateUI(true);
            }
            else
            {
                // Signed out, show unauthenticated UI.
                UpdateUI(false);
            }
        }
        // [END handleSignInResult]

        // [START signIn]
        private void SignIn()
        {
            Intent signInIntent = Auth.GoogleSignInApi.GetSignInIntent(_mGoogleApiClient);
            StartActivityForResult(signInIntent, RC_SIGN_IN);
        }
        // [END signIn]

        private void ShowProgressDialog()
        {
            if (_mProgressDialog == null)
            {
                _mProgressDialog = new ProgressDialog(this);
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

        private void UpdateUI(bool signedIn)
        {
            if (signedIn)
            {
                FindViewById(Resource.Id.sign_in_button).Visibility = ViewStates.Gone;
                FindViewById(Resource.Id.sign_out_and_disconnect).Visibility = ViewStates.Visible;

                _nextActivity = new Intent(this, typeof(MainActivity));
                ((MainApplication)Application).GSC = _acct;

                StartActivity(_nextActivity);
            }
            else
            {
                _mStatusTextView.SetText(Resource.String.signed_out);

                FindViewById(Resource.Id.sign_in_button).Visibility = ViewStates.Visible;
                FindViewById(Resource.Id.sign_out_and_disconnect).Visibility = ViewStates.Gone;
            }
        }

        protected override async void OnStart()
        {
            base.OnStart();

            OptionalPendingResult opr = Auth.GoogleSignInApi.SilentSignIn(_mGoogleApiClient);
            if (opr.IsDone)
            {
                // If the user's cached credentials are valid, the OptionalPendingResult will be "done"
                // and the GoogleSignInResult will be available instantly.
                Log.Debug(TAG, "Got cached sign-in");
                GoogleSignInResult result = (GoogleSignInResult)opr.Get();
                HandleSignInResult(result);
            }
            else
            {
                // If the user has not previously signed in on this device or the sign-in has expired,
                // this asynchronous branch will attempt to sign in the user silently.  Cross-device
                // single sign-on will occur in this branch.
                ShowProgressDialog();
                var result = await Auth.GoogleSignInApi.SilentSignIn(_mGoogleApiClient).AsAsync<GoogleSignInResult>();

                HideProgressDialog();
                HandleSignInResult(result);
            }
        }

        // [START signOut]
        static async void SignOut()
        {
            var result = await Auth.GoogleSignInApi.SignOut(_mGoogleApiClient);
            if (result.Status.IsSuccess)
            {
                //UpdateUI(false);
            }
        }
        // [END signOut]

        public void OnClick(View v)
        {
            switch (v.Id)
            {
                case Resource.Id.sign_in_button:
                    SignIn();
                    break;
                case Resource.Id.sign_out_button:
                    SignOut();
                    break;
                case Resource.Id.disconnect_button:
                    RevokeAccess();
                    break;
            }
        }
        // [START revokeAccess]
        private async void RevokeAccess()
        {
            var result = await Auth.GoogleSignInApi.RevokeAccess(_mGoogleApiClient);
            if (result.Status.IsSuccess)
            {
                // [START_EXCLUDE]
                UpdateUI(false);
                // [END_EXCLUDE]
            }
        }
        // [END revokeAccess]

        public GoogleApiClient GetGoogleApiClient()
        {
            return _mGoogleApiClient;
        }
    }
}