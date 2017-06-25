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
using System.Net;
using FinalYearProject.Domain;
using FinalYearProject.Mobile.Services;
using Android.Util;

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
            _searchButton.Click += Search;

            return v;
        }

        private async void Search(object sender, EventArgs e)
        {
            APIService serv = new APIService();
            var term = _searchTermTextView.Text;
            var onlineStores = await serv.SearchBySearchTerm(term);
            var offlineStores = await serv.SearchBySearchTermOffline(term);

            offlineStores = offlineStores.Where(s => s.Address1 != null).ToList();
            onlineStores = onlineStores.Where(s => s.Url != null).ToList();
            SetOnlineStores(onlineStores);
            SetOfflineStores(offlineStores);

            Fragment frag = ProductListingFragment.NewInstance();
            
            Activity.SupportFragmentManager.BeginTransaction()
            .Replace(Resource.Id.content_frame, frag)
            .AddToBackStack(null)
            .Commit();
        }
        
        private void SetOfflineStores(List<OfflineStore> offlineStores)
        {
            try
            {
                if (offlineStores != null)
                {
                    var myActivity = (MainActivity)Activity;
                    myActivity.SetOfflineStores(offlineStores);
                }

            }
            catch (Exception ex)
            {
                Log.Debug("Set OfflineStores Fail", ex.ToString());
            }
        }

        private void SetOnlineStores(List<OnlineStore> onlineStores)
        {
            try
            {
                if (onlineStores != null)
                {
                    var myActivity = (MainActivity)Activity;
                    myActivity.SetOnlineStores(onlineStores);
                }

            }
            catch (Exception ex)
            {
                Log.Debug("Set OnlineStores Fail", ex.ToString());
            }
        }
    

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

        }
        
        //private async void DownloadDataAsync()
        //{
        //    _searchTerm = _searchTermTextView.Text;
        //    string url = "https://locations.where-to-buy.co/api/local/all?SamsungExperienceStore=0&distance=50000&latitude=52.37403&locale=nl-NL&longitude=4.88969&model=SM-G928FZDAPHN&orderBy=category&token=Z58QH8nvuxDQlHRDCC7M4QmLtbc3MvmvCturJOT1m3TALfCTlOH47dz96oelTUOD5sV%2FHtIGfy%2BM6xrHsNvwEa92mdJ85Ak7VBowdYajinI%3D&top=50";

        //    var httpClient = new HttpClient();
        //    Task<string> downloadTask = httpClient.GetStringAsync(url);
        //    string content = await downloadTask;
        //    Console.Out.WriteLine("Response: \r\n {0}", content);

        //    var store = new List<Product>();
        //    JObject jsonResponse = JObject.Parse(content);
        //    IList<JToken> results = jsonResponse["Stores"].ToList();
        //    foreach (JToken token in results)
        //    {
        //        Product poi = JsonConvert.DeserializeObject<Product>(token.ToString());
        //        store.Add(poi);
        //    }
        //    var y = store;
        //}
    }
}