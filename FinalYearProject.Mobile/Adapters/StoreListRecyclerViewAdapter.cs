using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using FinalYearProject.Domain;

namespace FinalYearProject.Mobile.Adapters
{
    class StoreListRecyclerViewAdapter : RecyclerView.Adapter
    {
        public event EventHandler<StoreListRecyclerViewAdapterClickEventArgs> ItemClick;
        public event EventHandler<StoreListRecyclerViewAdapterClickEventArgs> ItemLongClick;
        List<Store> _stores;

        public StoreListRecyclerViewAdapter(List<Store> stores)
        {
            _stores = stores;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.storeListRow;
            itemView = LayoutInflater.From(parent.Context).
            Inflate(id, parent, false);

            var vh = new StoreListRecyclerViewAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = _stores[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as StoreListRecyclerViewAdapterViewHolder;
            holder.Description.Text = item.Name.ToString();
            holder.StoreType.Text = item.StoreLocationOptions.Find(x => x.Name == "StoreType").Value.ToString();
            //var p = y.StoreOptions.Where(x => x.Name == "StoreType" && x.Value == "Online");
        }

        public override int ItemCount => _stores.Count;

        void OnClick(StoreListRecyclerViewAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(StoreListRecyclerViewAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

        
    }

    public class StoreListRecyclerViewAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView Description { get; set; }
        public TextView StoreType { get; set; }

        public StoreListRecyclerViewAdapterViewHolder(View itemView, Action<StoreListRecyclerViewAdapterClickEventArgs> clickListener,
                            Action<StoreListRecyclerViewAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            Description = itemView.FindViewById<TextView>(Resource.Id.storeListRowTextView);
            StoreType = itemView.FindViewById<TextView>(Resource.Id.storeListRowTextViewStoreType);
            itemView.Click += (sender, e) => clickListener(new StoreListRecyclerViewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new StoreListRecyclerViewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class StoreListRecyclerViewAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}