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
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FinalYearProject.Mobile.Services
{
    public interface IRESTService
    {
        Task<string> SearchByEAN(string ean);
        Task<bool> UserExists(string id);
        Task<bool> SaveEvent(JObject ev);
    }
}