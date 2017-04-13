﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;

//namespace FinalYearProject.Mobile.Adapters
//{
//    public class ExpandableRecyclerAdapterHelper
//    {
//        const int InitialStableId = 0;

//        List<Object> _helperItemList;
//        static int _currentId;

//        public ExpandableRecyclerAdapterHelper(List<Object> itemList)
//        {
//            _currentId = InitialStableId;
//            _helperItemList = GenerateHelperItemList(itemList);
//        }

//        public List<Object> HelperItemList
//        {
//            get { return _helperItemList; }
//        }

//        public Object GetHelperItemAtPosition(int position)
//        {
//            return _helperItemList[position];
//        }

//        public List<Object> GenerateHelperItemList(List<object> itemList)
//        {
//            var parentWrapperList = new List<Object>();
//            foreach (var item in itemList)
//            {
//                if (item is IParentObject)
//                {
//                    var parentWrapper = new ParentWrapper(item, _currentId);
//                    _currentId++;
//                    parentWrapperList.Add(parentWrapper);
//                }
//                else
//                {
//                    parentWrapperList.Add(item);
//                }
//            }

//            return parentWrapperList;
//        }
//    }
//}