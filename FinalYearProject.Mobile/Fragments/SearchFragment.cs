using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using FinalYearProject.Mobile.Activities;
using FinalYearProject.Mobile.Helpers;
using Plugin.Geolocator.Abstractions;

namespace FinalYearProject.Mobile.Fragments
{
    public class SearchFragment : Fragment
    {
        private Button _searchButton;
        private AutoCompleteTextView _searchTermTextView;
        private string _searchTerm;
        Intent _nextActivity;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //_pos = LocationHelper.GetLocation();
        }

        public static SearchFragment NewInstance()
        {
            var frag1 = new SearchFragment { Arguments = new Bundle() };
            return frag1;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            //var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View v = inflater.Inflate(Resource.Layout.searchFragment, container, false);
            _searchButton = v.FindViewById<Button>(Resource.Id.buttonSearch);
            _searchTermTextView = v.FindViewById<AutoCompleteTextView>(Resource.Id.autoCompleteTextViewSearch);
            _searchButton.Click += please;

            return v;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            //var ignored = base.OnCreateView(inflater, container, savedInstanceState);

        }

        private void please(object sender, EventArgs e)
        {

            _searchTerm = _searchTermTextView.Text;
            _nextActivity = new Intent(this.Activity, typeof(ProductListingsActivity));
            _nextActivity.PutExtra("searchTerm", _searchTerm);
            StartActivity(_nextActivity);
        }
    }
}