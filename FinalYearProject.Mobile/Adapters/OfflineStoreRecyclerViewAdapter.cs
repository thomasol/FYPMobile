using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using FinalYearProject.Domain;

namespace FinalYearProject.Mobile.Adapters
{
    class OfflineStoreRecyclerViewAdapter : RecyclerView.Adapter
    {
        public event EventHandler<OfflineStoreRecyclerViewAdapterClickEventArgs> ItemClick;
        public event EventHandler<OfflineStoreRecyclerViewAdapterClickEventArgs> ItemLongClick;
        List<LocalStore> _stores;

        public OfflineStoreRecyclerViewAdapter(List<LocalStore> stores)
        {
            _stores = stores;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.offlineStoreListRow;
            itemView = LayoutInflater.From(parent.Context).
            Inflate(id, parent, false);

            var vh = new OfflineStoreRecyclerViewAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = _stores[position];
            var holder = viewHolder as OfflineStoreRecyclerViewAdapterViewHolder;
            holder.Description.Text = item.Description.ToString();
            holder.Name.Text = item.Name.ToString();
            holder.Price.Text = "€" + item.Price.ToString();
            holder.Address.Text = item.Address.ToString();
        }

        public override int ItemCount => _stores.Count;

        void OnClick(OfflineStoreRecyclerViewAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(OfflineStoreRecyclerViewAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);
        
    }

    public class OfflineStoreRecyclerViewAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView Description { get; set; }
        public TextView StoreType { get; set; }
        public TextView Country { get; set; }
        public TextView StoreCode { get; set; }
        public TextView Name { get; set; }
        public TextView Address { get; set; }
        public TextView Price { get; set; }
        public TextView Stock { get; set; }
        public OfflineStoreRecyclerViewAdapterViewHolder(View itemView, Action<OfflineStoreRecyclerViewAdapterClickEventArgs> clickListener,
                            Action<OfflineStoreRecyclerViewAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            Description = itemView.FindViewById<TextView>(Resource.Id.localStoreListRowTextViewDescription);
            //Country = itemView.FindViewById<TextView>(Resource.Id.localStoreListRowTextViewDetails);
            Name = itemView.FindViewById<TextView>(Resource.Id.localStoreName);
            Address = itemView.FindViewById<TextView>(Resource.Id.localStoreListRowTextViewAddress);
            Price = itemView.FindViewById<TextView>(Resource.Id.localStoreListRowTextViewPrice);

            itemView.Click += (sender, e) => clickListener(new OfflineStoreRecyclerViewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new OfflineStoreRecyclerViewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class OfflineStoreRecyclerViewAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}