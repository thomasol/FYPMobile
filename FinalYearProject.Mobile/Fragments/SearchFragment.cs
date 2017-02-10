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
using FinalYearProject.Mobile.Helpers;
using Plugin.Geolocator.Abstractions;

namespace FinalYearProject.Mobile.Fragments
{
    public class SearchFragment : Fragment, View.IOnClickListener
    {
        private Button _searchButton;
        private AutoCompleteTextView _searchTermTextView;
        private string _searchTerm;
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
            return inflater.Inflate(Resource.Layout.searchFragment, null);
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            //var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            
            _searchButton.SetOnClickListener(this);
        }
        private void HandleSearchButton()
        {
            _searchTerm = _searchTermTextView.Text;
        }

        public void OnClick(View v)
        {
            switch (v.Id)
            {
                case Resource.Id.buttonSearch:
                {
                    _searchTermTextView = v.FindViewById<AutoCompleteTextView>(Resource.Id.autoCompleteTextViewSearch);
                    HandleSearchButton();
                    break;
                }
            }
        }
    }
}