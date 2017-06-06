using System;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using FinalYearProject.Domain;

namespace FinalYearProject.Mobile.Adapters
{
    class LocalStoreRecyclerViewAdapter : RecyclerView.Adapter
    {
        public event EventHandler<LocalStoreRecyclerViewAdapterClickEventArgs> ItemClick;
        public event EventHandler<LocalStoreRecyclerViewAdapterClickEventArgs> ItemLongClick;
        List<LocalStore> _stores;

        public LocalStoreRecyclerViewAdapter(List<LocalStore> stores)
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

            var vh = new LocalStoreRecyclerViewAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = _stores[position];
            var holder = viewHolder as LocalStoreRecyclerViewAdapterViewHolder;
            holder.Description.Text = item.Description.ToString();
            holder.Name.Text = item.Name.ToString();
            holder.Price.Text = "€" + item.Price.ToString();
            holder.Address.Text = item.Address.ToString();
        }

        public override int ItemCount => _stores.Count;

        void OnClick(LocalStoreRecyclerViewAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(LocalStoreRecyclerViewAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);
        
    }

    public class LocalStoreRecyclerViewAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView Description { get; set; }
        public TextView StoreType { get; set; }
        public TextView Country { get; set; }
        public TextView StoreCode { get; set; }
        public TextView Name { get; set; }
        public TextView Address { get; set; }
        public TextView Price { get; set; }
        public TextView Stock { get; set; }
        public TextView Gps { get; set; }

        public LocalStoreRecyclerViewAdapterViewHolder(View itemView, Action<LocalStoreRecyclerViewAdapterClickEventArgs> clickListener,
                            Action<LocalStoreRecyclerViewAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            Description = itemView.FindViewById<TextView>(Resource.Id.localStoreListRowTextViewDescription);
            Name = itemView.FindViewById<TextView>(Resource.Id.localStoreName);
            Address = itemView.FindViewById<TextView>(Resource.Id.localStoreListRowTextViewAddress);
            Price = itemView.FindViewById<TextView>(Resource.Id.localStoreListRowTextViewPrice);
            //Stock = itemView.FindViewById<TextView>(Resource.Id.localStoreListRowTextViewStock);
            //Gps = itemView.FindViewById<TextView>(Resource.Id.localStoreListRowTextViewGps);

            itemView.Click += (sender, e) => clickListener(new LocalStoreRecyclerViewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new LocalStoreRecyclerViewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class LocalStoreRecyclerViewAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}