//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;
//using Android.Support.V4.App;
//using FinalYearProject.Domain;
//using FinalYearProject.Mobile.Fragments;

//namespace FinalYearProject.Mobile.Adapters
//{
//    class StoreListTabbedAdapter : FragmentPagerAdapter
//    {
//        // Underlying model data (flash card deck):
//        public Product _product;

//        // Constructor accepts a deck of flash cards:
//        public StoreListTabbedAdapter(Android.Support.V4.App.FragmentManager fm, Product product)
//            : base(fm)
//        {
//            this._product = product;
//        }

//        // Returns the number of cards in the deck:
//        public override int Count { get { return 1; } }

//        // Returns a new fragment for the flash card at this position:
//        public override Android.Support.V4.App.Fragment GetItem(int position)
//        {
//            return (Android.Support.V4.App.Fragment)
//                StoreListFragment.newInstance(flashCardDeck[position].Problem, flashCardDeck[position].Answer);
//        }

//        // Display the problem number in the PagerTitleStrip:
//        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
//        {
//            return new Java.Lang.String("Problem " + (position + 1));
//        }
//    }
//}