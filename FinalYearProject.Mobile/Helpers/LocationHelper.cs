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
        public static async Task<Position> GetLocation()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 500;

            Position position = await locator.GetPositionAsync(10000);

            return position;
        }
    }

}