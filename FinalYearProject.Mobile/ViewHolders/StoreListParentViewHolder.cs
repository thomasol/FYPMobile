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

namespace FinalYearProject.Mobile.ViewHolders
{
    public class StoreListParentViewHolder : ParentViewHolder
    {
        public TextView _crimeTitleTextView;
        public ImageButton _parentDropDownArrow;
        public StoreListParentViewHolder(View itemView) : base(itemView)
        {
            //_crimeTitleTextView = itemView.FindViewById<TextView>(Resource.Id.parent_list_item_crime_title_text_view);
            //_parentDropDownArrow = itemView.FindViewById<ImageButton>(Resource.Id.parent_list_item_expand_arrow);
        }
    }
}