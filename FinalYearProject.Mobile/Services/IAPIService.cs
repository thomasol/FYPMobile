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
using FinalYearProject.Domain;

namespace FinalYearProject.Mobile.Services
{
    public interface IAPIService
    {
        Task<bool> SaveEvent(JObject ev);
        Task<List<OnlineStore>> SearchByEAN(string ean);
        Task<List<OfflineStore>> SearchByEANOffline(string ean);
        Task<List<OnlineStore>> SearchBySearchTerm(string term);
        Task<List<OfflineStore>> SearchBySearchTermOffline(string term);
        Task<User> CheckUser(string id);
        Task UpdateUser(JObject ev);
        Task<User> AddUser(JObject user);
        Task<User> GetUser(string id);
    }
}