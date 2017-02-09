using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;

namespace FinalYearProject.Mobile.Adapters
{
    class ProductListingRecyclerViewAdapter : RecyclerView.Adapter
    {
        public event EventHandler<ProductListingRecyclerViewAdapterClickEventArgs> ItemClick;
        public event EventHandler<ProductListingRecyclerViewAdapterClickEventArgs> ItemLongClick;
        private string[] _items;

        public ProductListingRecyclerViewAdapter(string[] data)
        {
            _items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            //var id = Resource.Layout.__YOUR_ITEM_HERE;
            //itemView = LayoutInflater.From(parent.Context).
            //       Inflate(id, parent, false);

            var vh = new ProductListingRecyclerViewAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = _items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as ProductListingRecyclerViewAdapterViewHolder;
            //holder.TextView.Text = items[position];
        }

        public override int ItemCount => _items.Length;

        void OnClick(ProductListingRecyclerViewAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(ProductListingRecyclerViewAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class ProductListingRecyclerViewAdapterViewHolder : RecyclerView.ViewHolder
    {
        //public TextView TextView { get; set; }


        public ProductListingRecyclerViewAdapterViewHolder(View itemView, Action<ProductListingRecyclerViewAdapterClickEventArgs> clickListener,
                            Action<ProductListingRecyclerViewAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            //TextView = v;
            itemView.Click += (sender, e) => clickListener(new ProductListingRecyclerViewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new ProductListingRecyclerViewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class ProductListingRecyclerViewAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}