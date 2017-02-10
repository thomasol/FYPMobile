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

namespace FinalYearProject.Mobile.Activities
{
    [Activity(Label = "ProductListingsActivity")]
    public class ProductListingsActivity : AppCompatActivity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ProductListings);

            string searchTerm = Intent.GetStringExtra("searchTerm") ?? "Data not available";
            TextView t = FindViewById<TextView>(Resource.Id.textView1);
            t.Text = searchTerm;
            if (LocationHelper.Position == null)
            {
                await LocationHelper.GetLocation();
            }
            var x = LocationHelper.Position;
            var w = x;
        }

        //public override bool OnCreateOptionsMenu(IMenu menu)
        //{
        //    MenuInflater.Inflate(Resource.Menu.actionBarMenu, menu);
        //    return base.OnPrepareOptionsMenu(menu);
        //}
    }
}