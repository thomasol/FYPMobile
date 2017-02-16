using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FinalYearProject.Domain;

namespace FinalYearProject.Mobile.Adapters
{
    class StoreListAdapter : BaseAdapter<Store>
    {

        Activity context;
        List<Store> list;

        public StoreListAdapter(Activity context, List<Store> _list)
        {
            this.context = context;
            list = _list;
        }


        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            // re-use an existing view, if one is available
            // otherwise create a new one
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.storeListRow, parent, false);

            Store item = this[position];
            //view.FindViewById<TextView>(Resource.Id.Title).Text = item.ProductOptions.Where(x => x.Name == "Description");
            //view.FindViewById<TextView>(Resource.Id.Description).Text = item.Description;

            //using (var imageView = view.FindViewById<ImageView>(Resource.Id.Thumbnail))
            //{
            //    string url = Android.Text.Html.FromHtml(item.thumbnail).ToString();

            //    //Download and display image
            //    Koush.UrlImageViewHelper.SetUrlDrawable(imageView,
            //        url, Resource.Drawable.Placeholder);
            //}
            return view;
        }

        public override int Count
        {
            get
            {
                return list.Count;
            }
        }

        public override Store this[int index]
        {
            get
            {
                return list[index];
            }
        }
    }

    class StoreListAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        //public TextView Title { get; set; }
    }
}