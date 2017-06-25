using System;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using FinalYearProject.Domain;
using Android.Graphics;
using System.Net;
using System.Linq;

namespace FinalYearProject.Mobile.Adapters
{
    class OnlineStoreRecyclerViewAdapter : RecyclerView.Adapter
    {
        public event EventHandler<OnlineStoreRecyclerViewAdapterClickEventArgs> ItemClick;
        public event EventHandler<OnlineStoreRecyclerViewAdapterClickEventArgs> ItemLongClick;
        List<OnlineStore> _onlineStores;

        public OnlineStoreRecyclerViewAdapter(List<OnlineStore> onlineStores)
        {
            _onlineStores = onlineStores;
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
            var item = _onlineStores[position];
            if(item.Url != null)
            {
                // Replace the contents of the view with that element
                var holder = viewHolder as OnlineStoreRecyclerViewAdapterViewHolder;
                holder.Description.Text = item.Description;
                //holder.Currency.Text = item[position].Currency.ToString();
                holder.Stock.Text = item.Stock;
                holder.Price.Text = item.Currency + " " + item.Price;
                holder.Name.Text = item.Name;
                holder.Url.Text = item.Url ?? "";
            }
        }

        public override int ItemCount => _onlineStores.Count;

        void OnClick(OnlineStoreRecyclerViewAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(OnlineStoreRecyclerViewAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);


        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;
            using (var webClient = new WebClient())
            {
                webClient.UseDefaultCredentials = true;
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }
            return imageBitmap;   
        }
    }

    public class OnlineStoreRecyclerViewAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView Description { get; set; }
        public TextView StoreType { get; set; }
        public TextView StoreCode { get; set; }
        public TextView Name { get; set; }
        public TextView Url { get; set; }
        public TextView Stock { get; set; }
        public TextView Price { get; set; }
        public TextView Ean { get; set; }
        public TextView Currency { get; set; }
        public TextView Brand { get; set; }
        public TextView BrandId { get; set; }
        public ImageView BrandLogo { get; set; }
        public ImageView ImageUrl { get; set; }
        public TextView MPN { get; set; }
        public TextView Retailer { get; set; }
        public ImageView RetailerLogo { get; set; }
        public TextView Sku { get; set; }
        public TextView UPC { get; set; }

        public OnlineStoreRecyclerViewAdapterViewHolder(View itemView, Action<OnlineStoreRecyclerViewAdapterClickEventArgs> clickListener,
                            Action<OnlineStoreRecyclerViewAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            Description = itemView.FindViewById<TextView>(Resource.Id.onlineStoreListRowTextViewDescription);
            BrandLogo = itemView.FindViewById<ImageView>(Resource.Id.onlineStoreListRowImageViewBrandLogo);
            Name = itemView.FindViewById<TextView>(Resource.Id.onlineStoreListRowTextViewName);
            Url = itemView.FindViewById<TextView>(Resource.Id.onlineStoreListRowTextViewUrl);
            Price = itemView.FindViewById<TextView>(Resource.Id.onlineStoreListRowTextViewPrice);
            Stock = ItemView.FindViewById<TextView>(Resource.Id.onlineStoreListRowTextViewStock);
            //Currency = itemView.FindViewById<TextView>(Resource.Id.)
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