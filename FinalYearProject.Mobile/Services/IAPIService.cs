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
using Android.Gms.Auth.Api.SignIn;

namespace FinalYearProject.Mobile.Services
{
    public interface IAPIService
    {
        Task<string> SearchByEAN(string ean);
        Task<bool> SaveEvent(JObject ev);
        Task<string> CheckUser(GoogleSignInAccount _acct);
    }
}