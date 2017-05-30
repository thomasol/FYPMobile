using System;
using Android.Content;
using Android.Gms.Auth.Api.SignIn;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Uri = Android.Net.Uri;
using Android.Graphics;
using System.Net;
using static Android.Graphics.Bitmap;
using static Android.Graphics.PorterDuff;

namespace FinalYearProject.Mobile.Fragments
{
    public class AccountFragment : Fragment
    {
        private TextView _mAccountNameTextView;
        private TextView _mAccountEmailTextView;
        private ImageView _mAccountPhotoImageButton;
        private GoogleSignInAccount _acct;
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
            //var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View v = inflater.Inflate(Resource.Layout.accountFragment, container, false);
            _mAccountNameTextView = (TextView)v.FindViewById(Resource.Id.accountName);
            _mAccountNameTextView.Text = _acct.DisplayName;

            string personEmail = "Email: " +_acct.Email;
            _mAccountEmailTextView = (TextView)v.FindViewById(Resource.Id.accountEmail);
            _mAccountEmailTextView.Text = personEmail;

            string personId = _acct.Id;

            Uri imageUri = _acct.PhotoUrl;
            string imageLocation = "https:" + imageUri.SchemeSpecificPart;
            var imageBitmap = GetImageBitmapFromUrl(imageLocation);

            _mAccountPhotoImageButton = (ImageView)v.FindViewById(Resource.Id.user_profile_photo);
            _mAccountPhotoImageButton.SetImageBitmap(imageBitmap);
            return v;
        }
        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;
            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
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
                bitmap.Height, Config.Argb8888);
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
    }
}