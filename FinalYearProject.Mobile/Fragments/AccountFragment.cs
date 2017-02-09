using System;
using Android.Content;
using Android.Gms.Auth.Api.SignIn;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using FinalYearProject.Mobile.Activities;
using Uri = Android.Net.Uri;

namespace FinalYearProject.Mobile.Fragments
{
    public class AccountFragment : Fragment
    {
        private TextView _mAccountNameTextView;
        private TextView _mAccountEmail;

        private GoogleSignInAccount _acct;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Bundle bundle = Arguments;
            if (bundle != null)
            {
                _acct = (GoogleSignInAccount)bundle.GetParcelable("account");
            }
            // Create your fragment here
        }

        public static AccountFragment NewInstance()
        {
            var frag1 = new AccountFragment { Arguments = new Bundle() };
            return frag1;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            //var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View v = inflater.Inflate(Resource.Layout.accountFragment, container, false);
            _mAccountNameTextView = (TextView)v.FindViewById(Resource.Id.accountName);
            _mAccountNameTextView.Text = _acct.DisplayName;

            string personEmail = _acct.Email;
            _mAccountEmail = (TextView)v.FindViewById(Resource.Id.accountEmail);
            _mAccountEmail.Text = personEmail;
            string personId = _acct.Id;
            Uri personPhoto = _acct.PhotoUrl;
            return v;
            //return inflater.Inflate(Resource.Layout.accountFragment, null);
        }
    }
}