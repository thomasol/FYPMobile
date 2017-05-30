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

namespace FinalYearProject.Mobile.Fragments
{
    public class OnlineStoresFragment : Android.Support.V4.App.Fragment
    {
        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        OnlineStoreRecyclerViewAdapter mAdapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            //return base.OnCreateView(inflater, container, savedInstanceState);
            View v = inflater.Inflate(Resource.Layout.onlineStoreFragment, container, false);
            mRecyclerView = v.FindViewById<RecyclerView>(Resource.Id.recyclerViewOnlineStores);

            mLayoutManager = new LinearLayoutManager(this.Activity);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            mRecyclerView.SetItemAnimator(new DefaultItemAnimator());
            // Plug the adapter into the RecyclerView:
            var myActivity = (ProductListingsActivity)Activity;
            var p = myActivity.GetProduct();
            mAdapter = new OnlineStoreRecyclerViewAdapter(p);
            mAdapter.ItemClick += OnItemClick;

            mRecyclerView.SetAdapter(mAdapter);
            return v;
        }

        private void OnItemClick(object sender, OnlineStoreRecyclerViewAdapterClickEventArgs e)
        {
            var y = e.Position;
        }
    }
}