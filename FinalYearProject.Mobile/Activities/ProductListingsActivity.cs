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
using FinalYearProject.Mobile;
using Android.Support.V7.App;
using FinalYearProject.Mobile.Helpers;
using Android.Support.V4.View;
using FinalYearProject.Mobile.Services;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace FinalYearProject.Mobile.Activities
{
    [Activity(Label = "ProductListingsActivity")]
    public class ProductListingsActivity : AppCompatActivity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            await CreateSearchEvent();

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ProductListings);

            string result = Intent.GetStringExtra("product") ?? "Data not available";
            TextView t = FindViewById<TextView>(Resource.Id.textView1);
            t.Text = result;
        }

        private async Task<bool> CreateSearchEvent()
        {
            IRESTService restService = new RESTService();
            string ean = Intent.GetStringExtra("ean") ?? "Data not available";
            dynamic fypSearchRequest = new JObject();

            //fypSearchRequest.searchTerm = ean;
            fypSearchRequest.CreatedAt = DateTime.Now;
            fypSearchRequest.Description = "Search Event";
            fypSearchRequest.Type = 0;
            //fypSearchRequest.UserId = ...
            var value = await restService.SaveEvent(fypSearchRequest);
            return value;
        }

        Android.Support.V7.Widget.ShareActionProvider _actionProvider;

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_share1, menu);

            var shareItem = menu.FindItem(Resource.Id.action_share);
            var provider = MenuItemCompat.GetActionProvider(shareItem);

            _actionProvider = provider.JavaCast<Android.Support.V7.Widget.ShareActionProvider>();

            var intent = new Intent(Intent.ActionSend);
            intent.SetType("text/plain");
            intent.PutExtra(Intent.ExtraText, "Time to share some text!");

            _actionProvider.SetShareIntent(intent);

            return base.OnPrepareOptionsMenu(menu);
        }
    }
}