using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V4.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System.Net.Http;
using FinalYearProject.Domain;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Android.Support.V7.Widget;
using FinalYearProject.Mobile.Adapters;

namespace FinalYearProject.Mobile.Fragments
{
    public class StoreListFragment : Fragment
    {
        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        StoreListRecyclerViewAdapter mAdapter;
        List<Product> stores;
        public override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            stores = await DownloadDataAsync();

            mLayoutManager = new LinearLayoutManager(this.Activity);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            // Plug the adapter into the RecyclerView:
            mAdapter = new StoreListRecyclerViewAdapter(stores);
            mAdapter.ItemClick += OnItemClick;
            mRecyclerView.SetAdapter(mAdapter);
            // Create your fragment here
        }

        private void OnItemClick(object sender, StoreListRecyclerViewAdapterClickEventArgs e)
        {
            var y = stores[e.Position];
            
        }

        public static StoreListFragment NewInstance()
        {
            var frag1 = new StoreListFragment { Arguments = new Bundle() };
            return frag1;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View v = inflater.Inflate(Resource.Layout.storeListFragment, container, false);
            mRecyclerView = v.FindViewById<RecyclerView>(Resource.Id.recyclerViewStoreList);

            return v;
        }

        private async Task<List<Product>> DownloadDataAsync()
        {
            //_searchTerm = _searchTermTextView.Text;
            string url = "https://locations.where-to-buy.co/api/local/all?SamsungExperienceStore=0&distance=50000&latitude=52.37403&locale=nl-NL&longitude=4.88969&model=SM-G928FZDAPHN&orderBy=category&token=Z58QH8nvuxDQlHRDCC7M4QmLtbc3MvmvCturJOT1m3TALfCTlOH47dz96oelTUOD5sV%2FHtIGfy%2BM6xrHsNvwEa92mdJ85Ak7VBowdYajinI%3D&top=50";

            var httpClient = new HttpClient();
            Task<string> downloadTask = httpClient.GetStringAsync(url);
            string content = await downloadTask;
            Console.Out.WriteLine("Response: \r\n {0}", content);

            var store = new List<Product>();
            JObject jsonResponse = JObject.Parse(content);
            IList<JToken> results = jsonResponse["Stores"].ToList();
            foreach (JToken token in results)
            {
                Product poi = JsonConvert.DeserializeObject<Product>(token.ToString());
                store.Add(poi);
            }
            return store;
        }
    }
}