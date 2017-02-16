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
using System.Net;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using FinalYearProject.Domain;
using Newtonsoft.Json;

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
            _searchButton.Click += FetchLocationDataAsync;

            return v;
        }

        private void FetchLocationDataAsync(object sender, EventArgs e)
        {
            DownloadDataAsync();
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            //var ignored = base.OnCreateView(inflater, container, savedInstanceState);

        }

        //private async void please(object sender, EventArgs e)
        //{
        //    _searchTerm = _searchTermTextView.Text;
        //    string url = "https://locations.where-to-buy.co/api/local/all?SamsungExperienceStore=0&distance=50000&latitude=52.37403&locale=nl-NL&longitude=4.88969&model=SM-G928FZDAPHN&orderBy=category&token=Z58QH8nvuxDQlHRDCC7M4QmLtbc3MvmvCturJOT1m3TALfCTlOH47dz96oelTUOD5sV%2FHtIGfy%2BM6xrHsNvwEa92mdJ85Ak7VBowdYajinI%3D&top=50";

        //    // Fetch the weather information asynchronously, 
        //    // parse the results, then update the screen:
        //    JValue json = await FetchLocationDataAsync(url);
        //    // ParseAndDisplay (json);

        //    _nextActivity = new Intent(this.Activity, typeof(ProductListingsActivity));
        //    _nextActivity.PutExtra("searchTerm", _searchTerm);
        //    StartActivity(_nextActivity);
        //}

        private async void DownloadDataAsync()
        {
            _searchTerm = _searchTermTextView.Text;
            string url = "https://locations.where-to-buy.co/api/local/all?SamsungExperienceStore=0&distance=50000&latitude=52.37403&locale=nl-NL&longitude=4.88969&model=SM-G928FZDAPHN&orderBy=category&token=Z58QH8nvuxDQlHRDCC7M4QmLtbc3MvmvCturJOT1m3TALfCTlOH47dz96oelTUOD5sV%2FHtIGfy%2BM6xrHsNvwEa92mdJ85Ak7VBowdYajinI%3D&top=50";

            var httpClient = new HttpClient();
            Task<string> downloadTask = httpClient.GetStringAsync(url);
            string content = await downloadTask;
            Console.Out.WriteLine("Response: \r\n {0}", content);

            var store = new List<Store>();
            JObject jsonResponse = JObject.Parse(content);
            IList<JToken> results = jsonResponse["Stores"].ToList();
            foreach (JToken token in results)
            {
                Store poi = JsonConvert.DeserializeObject<Store>(token.ToString());
                store.Add(poi);
            }
            var y = store;
        }
    }
}