using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using FinalYearProject.Mobile.Adapters;
using FinalYearProject.Mobile.Activities;

namespace FinalYearProject.Mobile.Fragments
{
    public class OfflineStoresFragment : Android.Support.V4.App.Fragment
    {
        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        OfflineStoreRecyclerViewAdapter mAdapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.offlineStoreFragment, container, false);
            mRecyclerView = v.FindViewById<RecyclerView>(Resource.Id.recyclerViewOfflineStores);

            mLayoutManager = new LinearLayoutManager(this.Activity);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            mRecyclerView.SetItemAnimator(new DefaultItemAnimator());
            // Plug the adapter into the RecyclerView:
            var myActivity = (ProductListingsActivity)Activity;
            var p = myActivity.GetProduct();
            mAdapter = new OfflineStoreRecyclerViewAdapter(p.LocalStores);
            mAdapter.ItemClick += OnItemClick;

            mRecyclerView.SetAdapter(mAdapter);
            return v;
        }

        private void OnItemClick(object sender, OfflineStoreRecyclerViewAdapterClickEventArgs e)
        {
            var y = e.Position;
        }
    }
}