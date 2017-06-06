using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using FinalYearProject.Domain;

namespace FinalYearProject.Mobile.Adapters
{
    class OnlineStoreRecyclerViewAdapter : RecyclerView.Adapter
    {
        public event EventHandler<OnlineStoreRecyclerViewAdapterClickEventArgs> ItemClick;
        public event EventHandler<OnlineStoreRecyclerViewAdapterClickEventArgs> ItemLongClick;
        Product _stores;

        public OnlineStoreRecyclerViewAdapter(Product stores)
        {
            _stores = stores;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.onlineStoreListRow;
            itemView = LayoutInflater.From(parent.Context).
            Inflate(id, parent, false);

            var vh = new OnlineStoreRecyclerViewAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = _stores;

            // Replace the contents of the view with that element
            var holder = viewHolder as OnlineStoreRecyclerViewAdapterViewHolder;
            holder.Description.Text = item.ModelNo.ToString();
            //holder.StoreType.Text = item.StoreLocationOptions.Find(x => x.Name == "StoreType").Value.ToString();
            //var p = y.StoreOptions.Where(x => x.Name == "StoreType" && x.Value == "Online");
        }

        public override int ItemCount => 1;

        void OnClick(OnlineStoreRecyclerViewAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(OnlineStoreRecyclerViewAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);
        
    }

    public class OnlineStoreRecyclerViewAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView Description { get; set; }
        public TextView StoreType { get; set; }
        public int StoreCode { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Stock { get; set; }
        public double? Price { get; set; }

        public OnlineStoreRecyclerViewAdapterViewHolder(View itemView, Action<OnlineStoreRecyclerViewAdapterClickEventArgs> clickListener,
                            Action<OnlineStoreRecyclerViewAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            Description = itemView.FindViewById<TextView>(Resource.Id.storeListRowTextViewDetails);
            StoreType = itemView.FindViewById<TextView>(Resource.Id.storeListRowTextViewStoreType);
            //StoreCode = itemView.FindViewById<TextView>(Resource.Id.storeListRowTextViewStoreCode);
            //Url = itemView.FindViewById<TextView>(Resource.Id.storeListRowTextViewUrl);
            //Stock = itemView.FindViewById<TextView>(Resource.Id.storeListRowTextViewStock);

            itemView.Click += (sender, e) => clickListener(new OnlineStoreRecyclerViewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new OnlineStoreRecyclerViewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class OnlineStoreRecyclerViewAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}