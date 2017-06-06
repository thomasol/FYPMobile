using System;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using FinalYearProject.Domain;

namespace FinalYearProject.Mobile.Adapters
{
    class ProductRecyclerViewAdapter : RecyclerView.Adapter
    {
        public event EventHandler<ProductRecyclerViewAdapterClickEventArgs> ItemClick;
        public event EventHandler<ProductRecyclerViewAdapterClickEventArgs> ItemLongClick;
        Product _stores;

        public ProductRecyclerViewAdapter(Product stores)
        {
            _stores = stores;
        }
        
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = null;
            var id = Resource.Layout.productRow;
            itemView = LayoutInflater.From(parent.Context).
            Inflate(id, parent, false);

            var vh = new ProductRecyclerViewAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }
        
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = _stores;

            // Replace the contents of the view with that element
            var holder = viewHolder as ProductRecyclerViewAdapterViewHolder;
            //holder.Description.Text = item.Description.ToString();
            //holder.BrandId.Text = item.BrandId.ToString();
            //holder.Upc.Text = item.Upc.ToString();
            //holder.Ean.Text = item.Ean.ToString();
        }

        public override int ItemCount => 1;

        void OnClick(ProductRecyclerViewAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(ProductRecyclerViewAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);
    }

    public class ProductRecyclerViewAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView Description { get; set; }
        public TextView ModelId { get; set; }
        //public TextView ModelNo { get; set; }
        public TextView Mpn { get; set; }
        public TextView Ean { get; set; }
        public TextView Upc { get; set; }
        public TextView BrandId { get; set; }
        public ProductRecyclerViewAdapterViewHolder(View itemView, Action<ProductRecyclerViewAdapterClickEventArgs> clickListener,
                            Action<ProductRecyclerViewAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            //Description = itemView.FindViewById<TextView>(Resource.Id.storeListRowTextView);
            //ModelId = itemView.FindViewById<TextView>(Resource.Id.store);
            Ean = itemView.FindViewById<TextView>(Resource.Id.productRowEan);
            itemView.Click += (sender, e) => clickListener(new ProductRecyclerViewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new ProductRecyclerViewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class ProductRecyclerViewAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}