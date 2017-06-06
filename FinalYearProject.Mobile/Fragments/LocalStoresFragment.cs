using System;
using Android.OS;
using Android.Views;
using Android.Support.V7.Widget;
using FinalYearProject.Mobile.Adapters;
using FinalYearProject.Mobile.Activities;
using FinalYearProject.Domain;
using FinalYearProject.Mobile.Services;
using Newtonsoft.Json.Linq;

namespace FinalYearProject.Mobile.Fragments
{
    public class LocalStoresFragment : Android.Support.V4.App.Fragment
    {
        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        LocalStoreRecyclerViewAdapter mAdapter;
        Product _p;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.localStoreTab, container, false);
            mRecyclerView = v.FindViewById<RecyclerView>(Resource.Id.recyclerViewOfflineStores);

            mLayoutManager = new LinearLayoutManager(this.Activity);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            mRecyclerView.SetItemAnimator(new DefaultItemAnimator());
            // Plug the adapter into the RecyclerView:
            var myActivity = (MainActivity)Activity;
            _p = myActivity.GetProduct();
            mAdapter = new LocalStoreRecyclerViewAdapter(_p.LocalStores);
            mAdapter.ItemClick += OnItemClick;

            mRecyclerView.SetAdapter(mAdapter);
            return v;
        }

        private async void OnItemClick(object sender, LocalStoreRecyclerViewAdapterClickEventArgs e)
        {
            int position = e.Position;
            var store = _p.LocalStores[position];

            IAPIService restService = new APIService();
            dynamic clickEvent = new JObject();

            var _clickGuid = Guid.NewGuid();
            clickEvent.ClickGuid = _clickGuid;
            clickEvent.StoreCode = store.StoreCode;
            Guid _impressionGuid = ((MainActivity)Activity).GetLocalImpressionGuid(position);
            clickEvent.ImpressionGuid = _impressionGuid;
            clickEvent.CreatedAt = DateTime.Now;
            clickEvent.Type = 2;

            await restService.SaveEvent(clickEvent);
        }
    }
}