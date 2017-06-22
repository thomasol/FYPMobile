using System;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using FinalYearProject.Domain;
using System.Linq;
namespace FinalYearProject.Mobile.Adapters
{
    class LocalStoreRecyclerViewAdapter : RecyclerView.Adapter
    {
        public event EventHandler<LocalStoreRecyclerViewAdapterClickEventArgs> ItemClick;
        public event EventHandler<LocalStoreRecyclerViewAdapterClickEventArgs> ItemLongClick;

        List<OfflineStore> _stores;

        public LocalStoreRecyclerViewAdapter(List<OfflineStore> stores)
        {
            _stores = stores.Where(s => s.Address1 != null).ToList();
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
            var x = item.Retailer;
            if (item.Address1 != null)
            {
                var holder = viewHolder as LocalStoreRecyclerViewAdapterViewHolder;
                holder.Description.Text = item.Description.ToString();
                holder.Name.Text = item.Name.ToString();
                holder.Price.Text = "€" + item.Price.ToString();
                holder.Address1.Text = item.Address1.ToString() ?? "";
                holder.Address2.Text = item.Address2.ToString() ?? "";
                holder.Address3.Text = item.Address3 ?? "";
                holder.Stock.Text = item.Stock.ToString();
            }
        }

        public override int ItemCount => _stores.Where(s => s.Address1 != null).Count();

        void OnClick(LocalStoreRecyclerViewAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(LocalStoreRecyclerViewAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);
        
    }

    public class LocalStoreRecyclerViewAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView Description { get; set; }
        public TextView Country { get; set; }
        public TextView StoreCode { get; set; }
        public TextView Name { get; set; }
        public TextView Address1 { get; set; }
        public TextView Address2 { get; set; }
        public TextView Address3 { get; set; }
        public TextView Price { get; set; }
        public TextView Stock { get; set; }
        public TextView Brand { get; set; }
        public int BrandId { get; set; }
        public ImageView BrandLogo { get; set; }
        public TextView City { get; set; }
        public TextView County { get; set; }
        public ImageView ImageUrl { get; set; }
        public string Mpn { get; set; }
        public TextView Retailer { get; set; }
        public ImageView RetailerLogo { get; set; }
        public string Sku { get; set; }
        public TextView Upc { get; set; }
        public Gps StoreCoordinates { get; set; }

        public LocalStoreRecyclerViewAdapterViewHolder(View itemView, Action<LocalStoreRecyclerViewAdapterClickEventArgs> clickListener,
                            Action<LocalStoreRecyclerViewAdapterClickEventArgs> longClickListener) : base(itemView)
        {

            Description = itemView.FindViewById<TextView>(Resource.Id.localStoreListRowTextViewDescription);
            Name = itemView.FindViewById<TextView>(Resource.Id.localStoreName);
            Address1 = itemView.FindViewById<TextView>(Resource.Id.localStoreListRowTextViewAddress1);
            Address2 = itemView.FindViewById<TextView>(Resource.Id.localStoreListRowTextViewAddress2);
            Address3 = itemView.FindViewById<TextView>(Resource.Id.localStoreListRowTextViewAddress3);
            Price = itemView.FindViewById<TextView>(Resource.Id.localStoreListRowTextViewPrice);
            Stock = itemView.FindViewById<TextView>(Resource.Id.localStoreListRowTextViewStock);
            RetailerLogo = itemView.FindViewById<ImageView>(Resource.Id.localStoreListRowImageViewRetailerLogo);

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