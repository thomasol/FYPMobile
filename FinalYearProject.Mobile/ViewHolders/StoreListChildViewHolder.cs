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
    public class StoreListChildViewHolder : ChildViewHolder
    {
        public TextView _crimeDateText;
        public CheckBox _crimeSolvedCheckBox;

        public StoreListChildViewHolder(View itemView) : base(itemView)
        {
            //_crimeDateText = itemView.FindViewById<TextView>(Resource.Id.child_list_item_crime_date_text_view);
            //_crimeSolvedCheckBox = itemView.FindViewById<CheckBox>(Resource.Id.child_list_item_crime_solved_check_box);
        }
    }
}