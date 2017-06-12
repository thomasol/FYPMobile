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
using Android.Support.V7.Widget;
using FinalYearProject.Mobile.Adapters;
using FinalYearProject.Mobile.Activities;
using FinalYearProject.Domain;
using FinalYearProject.Mobile.Services;
using Newtonsoft.Json.Linq;

namespace FinalYearProject.Mobile.Fragments
{
    public class OnlineStoresFragment : Android.Support.V4.App.Fragment
    {
        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        OnlineStoreRecyclerViewAdapter mAdapter;
        List<OnlineStore> _onlineStores;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View v = inflater.Inflate(Resource.Layout.onlineStoreTab, container, false);
            mRecyclerView = v.FindViewById<RecyclerView>(Resource.Id.recyclerViewOnlineStores);

            mLayoutManager = new LinearLayoutManager(this.Activity);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            mRecyclerView.SetItemAnimator(new DefaultItemAnimator());
            // Plug the adapter into the RecyclerView:
            var myActivity = (MainActivity)Activity;
            _onlineStores = myActivity.GetOnlineStores();
            mAdapter = new OnlineStoreRecyclerViewAdapter(_onlineStores);
            mAdapter.ItemClick += OnItemClick;

            mRecyclerView.SetAdapter(mAdapter);
            return v;
        }

        private async void OnItemClick(object sender, OnlineStoreRecyclerViewAdapterClickEventArgs e)
        {
            int position = e.Position;

            //uncomment and change
            //var onlineStore = _onlineStore.OnlineStores[position];
            var onlineStore = _onlineStores;

            string url = onlineStore[position].Url;
            var uri = Android.Net.Uri.Parse(url);
            var intent = new Intent(Intent.ActionView, uri);
            StartActivity(intent);

            IAPIService restService = new APIService();
            dynamic clickEvent = new JObject();

            var clickGUID = Guid.NewGuid();
            clickEvent.ClickGUID = clickGUID;
            Guid _impressionGuid = ((MainActivity)Activity).GetOnlineImpressionGuid(position);
            clickEvent.ImpressionGuid = _impressionGuid;
            clickEvent.CreatedAt = DateTime.Now;
            clickEvent.EAN = _onlineStores[position].Ean;
            clickEvent.Type = 2;

            await restService.SaveEvent(clickEvent);

        }
    }
}