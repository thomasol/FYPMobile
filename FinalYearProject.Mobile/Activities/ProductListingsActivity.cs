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
using FinalYearProject.Mobile.Adapters;
using FinalYearProject.Mobile.Fragments;
using Android.Support.Design.Widget;

namespace FinalYearProject.Mobile.Activities
{
    [Activity(Label = "ProductListingsActivity")]
    public class ProductListingsActivity : AppCompatActivity
    {

        private TabLayout tabLayout;
        private ViewPager viewPager;

        private int[] tabIcons = {
            Resource.Drawable.ic_store_white_24dp,
            Resource.Drawable.ic_store_white_24dp,
            Resource.Drawable.ic_store_white_24dp,
            Resource.Drawable.ic_store_white_24dp

        };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //await CreateSearchEvent();

            base.OnCreate(savedInstanceState);
            //SetContentView(Resource.Layout.storeListFragment);

            //string result = Intent.GetStringExtra("product") ?? "Data not available";
            //TextView t = FindViewById<TextView>(Resource.Id.textView1);
            //t.Text = result;
            SetContentView(Resource.Layout.ProductListings);
            viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            setupViewPager(viewPager);

            tabLayout = FindViewById<TabLayout>(Resource.Id.sliding_tabs);
            tabLayout.SetupWithViewPager(viewPager);
            setupTabIcons();
        }

        private void setupTabIcons()
        {
            tabLayout.GetTabAt(0).SetIcon(tabIcons[0]);
            tabLayout.GetTabAt(1).SetIcon(tabIcons[1]);
            //tabLayout.GetTabAt(2).SetIcon(tabIcons[2]);
            //tabLayout.GetTabAt(3).SetIcon(tabIcons[3]);
        }

        private OnlineStores _onlineStoresFrg;
        private OfflineStores _offlineStoresFrg;

        private void InditialFragment()
        {
            _onlineStoresFrg = new OnlineStores();
            _offlineStoresFrg = new OfflineStores();
            //moreFrg = new More();
            //todoFrag = new Todo();
        }
        public void setupViewPager(ViewPager viewPager)
        {
            InditialFragment();
            ViewPagerAdapter adapter = new ViewPagerAdapter(SupportFragmentManager);
            adapter.addFragment(_onlineStoresFrg, "Online");
            adapter.addFragment(_offlineStoresFrg, "Offline");

            viewPager.Adapter = adapter;
        }

        private async Task<bool> CreateSearchEvent()
        {
            IAPIService restService = new APIService();
            string ean = Intent.GetStringExtra("ean") ?? "Data not available";
            dynamic fypSearchRequest = new JObject();
            var searchGUID = Guid.NewGuid();
            //fypSearchRequest.searchTerm = ean;
            var pos = LocationHelper.Position;
            fypSearchRequest.CreatedAt = DateTime.Now;
            fypSearchRequest.Description = "Search Event";
            fypSearchRequest.Type = 0;
            fypSearchRequest.GUID = searchGUID;
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

            return base.OnPrepareOptionsMenu(menu);
        }
    }
}