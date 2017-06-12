using System;
using System.Collections.Generic;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using FinalYearProject.Domain;
using FinalYearProject.Mobile.Adapters;
using Fragment = Android.Support.V4.App.Fragment;
using Newtonsoft.Json.Linq;
using FinalYearProject.Mobile.Activities;
using System.Threading.Tasks;
using FinalYearProject.Mobile.Services;
using FinalYearProject.Mobile.Helpers;

namespace FinalYearProject.Mobile.Fragments
{
    public class ProductListingsFragment : Fragment
    {
        private TabLayout tabLayout;
        private ViewPager viewPager;

        private int[] tabIcons = {
            Resource.Drawable.ic_search_white_24dp,
            Resource.Drawable.ic_network_wifi_white_24dp,
            Resource.Drawable.ic_store_white_24dp
        };

        private SearchResultFragment _searchResultsFragment;
        private OnlineStoresFragment _onlineStoresFragment;
        private LocalStoresFragment _offlineStoresFragment;

        List<OnlineStore> _onlineStores;
        List<Guid> _localImpressionsGuid = new List<Guid>();
        List<Guid> _onlineImpressionsGuid = new List<Guid>();

        private const string TAG = "ProductListingsFragment";
        ViewPagerAdapter _adapter = null;

        public override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _onlineStores = ((MainActivity)Activity).GetOnlineStores();
            //await CreateImpressionEvent();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.ProductListings, container, false);

            viewPager = v.FindViewById<ViewPager>(Resource.Id.viewpager);
            viewPager.OffscreenPageLimit = 3;
            setupViewPager(viewPager);

            tabLayout = v.FindViewById<TabLayout>(Resource.Id.sliding_tabs);
            tabLayout.SetupWithViewPager(viewPager);
            setupTabIcons();
            return v;
        }

        public static ProductListingsFragment NewInstance()
        {
            var frag1 = new ProductListingsFragment { Arguments = new Bundle() };
            return frag1;
        }

        private void setupTabIcons()
        {
            tabLayout.GetTabAt(0).SetIcon(tabIcons[0]);
            tabLayout.GetTabAt(1).SetIcon(tabIcons[1]);
            tabLayout.GetTabAt(2).SetIcon(tabIcons[2]);
        }

        private void InitialiseFragments()
        {
            _searchResultsFragment = new SearchResultFragment();
            _onlineStoresFragment = new OnlineStoresFragment();
            _offlineStoresFragment = new LocalStoresFragment();
        }

        public void setupViewPager(ViewPager viewPager)
        {
            InitialiseFragments();
            _adapter = new ViewPagerAdapter(ChildFragmentManager);
            _adapter.addFragment(_searchResultsFragment, "Search Results");
            _adapter.addFragment(_onlineStoresFragment, "Online");
            _adapter.addFragment(_offlineStoresFragment, "Offline");

            viewPager.Adapter = _adapter;
        }

        private async Task CreateImpressionEvent()
        {
            try
            {
                IAPIService restService = new APIService();
                dynamic impressionEvent = new JObject();
                var pos = LocationHelper.Position;

                var _searchGuid = Guid.NewGuid();
                impressionEvent.SearchGuid = _searchGuid;
                impressionEvent.CreatedAt = DateTime.Now;
                impressionEvent.Lon = pos.Longitude;
                impressionEvent.Lat = pos.Latitude;
                impressionEvent.EAN = _onlineStores[0].Ean;
                impressionEvent.Type = 1;

                //uncomment change to local
                //for (int i = 0; i < _onlineStore.LocalStores.Count; i++)
                //{
                //    var impressionGuid = Guid.NewGuid();
                //    impressionEvent.ImpressionGuid = impressionGuid;
                //    impressionEvent.Description = "Search Event Local Store";
                //    impressionEvent.StoreCode = _onlineStore.LocalStores[i].StoreCode;
                //    _localImpressionsGuid.Add(impressionGuid);
                //    await restService.SaveEvent(impressionEvent);
                //}

                for (int i = 0; i < _onlineStores.Count; i++)
                {
                    var impressionGuid = Guid.NewGuid();
                    impressionEvent.ImpressionGuid = impressionGuid;
                    impressionEvent.Description = "Search Event Online Store";
                    impressionEvent.Url = _onlineStores[i].Url;
                    _onlineImpressionsGuid.Add(impressionGuid);
                    await restService.SaveEvent(impressionEvent);
                }

                ((MainActivity)Activity).SetLocalImpressionsGuid(_localImpressionsGuid);
                ((MainActivity)Activity).SetOnlineImpressionsGuid(_onlineImpressionsGuid);

            }
            catch (Exception ex)
            {
                Log.Debug(TAG, ex.ToString());
            }
        }

        public Guid GetLocalImpressionGuid(int position)
        {
            return _localImpressionsGuid[position];
        }

        public Guid GetOnlineImpressionGuid(int position)
        {
            return _onlineImpressionsGuid[position];
        }
    }
}