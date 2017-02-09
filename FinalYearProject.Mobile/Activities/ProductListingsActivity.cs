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

namespace FinalYearProject.Mobile.Activities
{
    [Activity(Label = "ProductListingsActivity")]
    public class ProductListingsActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            string text = Intent.GetStringExtra("location") ?? "Data not available";

            //SetContentView(Resource.Layout.ProductListings);
            
        }

        //public override bool OnCreateOptionsMenu(IMenu menu)
        //{
        //    MenuInflater.Inflate(Resource.Menu.actionBarMenu, menu);
        //    return base.OnPrepareOptionsMenu(menu);
        //}
    }
}