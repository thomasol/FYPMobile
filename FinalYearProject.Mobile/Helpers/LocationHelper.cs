using System;
using System.Threading.Tasks;
using Android;
using Android.Views;
using Android.Widget;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

namespace FinalYearProject.Mobile.Helpers
{
    public static class LocationHelper
    {
        private static Position _position;
        public static Position Position
        {
            get { return _position; }
        }
        public static async Task GetLocation()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 500;

            _position = await locator.GetPositionAsync(10000);

        }
    }
}