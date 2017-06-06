using Android.Gms.Auth.Api.SignIn;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Uri = Android.Net.Uri;
using Android.Graphics;
using FinalYearProject.Mobile.Activities;
using FinalYearProject.Mobile.Services;
using Newtonsoft.Json.Linq;
using System;

namespace FinalYearProject.Mobile.Fragments
{
    public class AccountFragment : Fragment
    {
        private TextView _mAccountNameTextView;
        private TextView _mAccountEmailTextView;
        private RadioButton _maleRadioButton;
        private RadioButton _femaleRadioButton;
        private TextView _mAccountAge;
        private ImageView _mAccountPhotoImageButton;
        private GoogleSignInAccount _acct;
        private Bitmap _image;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Activity.Title = "Account";
            _acct = ((MainApplication)Activity.Application).GSC;
        }

        public static AccountFragment NewInstance()
        {
            var frag1 = new AccountFragment { Arguments = new Bundle() };
            return frag1;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.accountFragment, container, false);
            _mAccountNameTextView = (TextView)v.FindViewById(Resource.Id.accountName);
            _mAccountNameTextView.Text = _acct.DisplayName;
            string personEmail = _acct.Email;
            _mAccountEmailTextView = (TextView)v.FindViewById(Resource.Id.accountEmail);
            _mAccountEmailTextView.Text = personEmail;
            _mAccountEmailTextView.Hint = "Email";

            _mAccountAge = (TextView)v.FindViewById(Resource.Id.accountAge);
            
            _maleRadioButton = (RadioButton)v.FindViewById(Resource.Id.radioMale);
            _femaleRadioButton = (RadioButton)v.FindViewById(Resource.Id.radioFemale);
            _maleRadioButton.Click += MaleRadioButtonClick;
            _femaleRadioButton.Click += FemaleRadioButtonClick;
            //if(_acct.gender != null)
            //{
            //    if(_acct.gender == "M")
            //    {
            //        maleRadioButton.Selected = true;
            //    }
            //    else
            //    {
            //        femaleRadioButton.Selected = true;
            //    }
            //}
            string personId = _acct.Id;
            if(_image == null)
            {
                Uri imageUri = _acct.PhotoUrl;
                string imageLocation = "https:" + imageUri.SchemeSpecificPart;
                _image = GetImageBitmapFromUrl();
                if(_image != null)
                {
                    _mAccountPhotoImageButton = (ImageView)v.FindViewById(Resource.Id.user_profile_photo);
                    _mAccountPhotoImageButton.SetImageBitmap(_image);
                }
            }
            else
            {
                _mAccountPhotoImageButton = (ImageView)v.FindViewById(Resource.Id.user_profile_photo);
                _mAccountPhotoImageButton.SetImageBitmap(_image);
            }

            Button submitButton = (Button)v.FindViewById(Resource.Id.submit);

            submitButton.Click += delegate
            {
                IAPIService apiService = new APIService();
                dynamic user = new JObject();
                user.Name = (string)_mAccountNameTextView.Text.ToString();
                user.Email = (string)_mAccountEmailTextView.Text.ToString();
                //user.Gender = (string)_mAccountGenderTextView.Text.ToString();
                user.Age = Convert.ToInt32((_mAccountAge.Text.ToString()));
                user.Id = _acct.Id;
                apiService.UpdateUser(user);
            };

            return v;
        }

        private void MaleRadioButtonClick(object sender, EventArgs e)
        {
            _maleRadioButton.Selected = true;
            _femaleRadioButton.Selected = false;
        }

        private void FemaleRadioButtonClick(object sender, EventArgs e)
        {
            _femaleRadioButton.Selected = true;
            _maleRadioButton.Selected = false;
        }

        private Bitmap GetImageBitmapFromUrl()
        {
            var bActivity = (BaseActivity)Activity;
            return bActivity.GetBitmap();
        }
    }
}