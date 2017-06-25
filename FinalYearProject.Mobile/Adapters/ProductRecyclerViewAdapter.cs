using System;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using FinalYearProject.Domain;
using System.Collections.Generic;
using Android.Graphics;
using System.Net;

namespace FinalYearProject.Mobile.Adapters
{
    class ProductRecyclerViewAdapter : RecyclerView.Adapter
    {
        public event EventHandler<ProductRecyclerViewAdapterClickEventArgs> ItemClick;
        public event EventHandler<ProductRecyclerViewAdapterClickEventArgs> ItemLongClick;
        List<OnlineStore> _onlineStores;

        public ProductRecyclerViewAdapter(List<OnlineStore> onlineStores)
        {
            _onlineStores = onlineStores;
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
            var item = _onlineStores[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as ProductRecyclerViewAdapterViewHolder;
            holder.Description.Text = item.Description;
            holder.Ean.Text = item.Ean;

            //var imageBitmap = GetImageBitmapFromUrl(item.BrandLogo);

            //holder.BrandLogo.SetImageBitmap(imageBitmap);
        }

        public override int ItemCount => 1;

        void OnClick(ProductRecyclerViewAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(ProductRecyclerViewAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

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

    public class ProductRecyclerViewAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView Description { get; set; }
        public TextView ModelId { get; set; }
        public TextView Mpn { get; set; }
        public TextView Ean { get; set; }
        public ImageView BrandLogo { get; set; }
        public TextView BrandId { get; set; }
        public ProductRecyclerViewAdapterViewHolder(View itemView, Action<ProductRecyclerViewAdapterClickEventArgs> clickListener,
                            Action<ProductRecyclerViewAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            Description = itemView.FindViewById<TextView>(Resource.Id.productDescription);
            //ModelId = itemView.FindViewById<TextView>(Resource.Id.store);
            Ean = itemView.FindViewById<TextView>(Resource.Id.productEan);
            BrandLogo = itemView.FindViewById<ImageView>(Resource.Id.productBrandLogo);

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