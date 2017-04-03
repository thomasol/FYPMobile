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
using System.Net.Http;
using Newtonsoft.Json;
using Android.Util;

namespace FinalYearProject.Mobile.Services
{
    public class RESTService: IRESTService
    {
        HttpClient httpClient = new HttpClient();

        public RESTService()
        {
            httpClient = new HttpClient();
            httpClient.MaxResponseContentBufferSize = 256000;
        }

        public async Task<bool> SaveEvent(JObject ev)
        {
            var jsonString = JsonConvert.SerializeObject(ev);

            var stringContent = new StringContent(jsonString, UnicodeEncoding.UTF8, "application/json");
            string url = "http://192.168.42.228:45456/api/Events/";
            try
            {
                var response = await httpClient.PostAsync(url, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("Event Save Error", ex.ToString());
            }
            return false;
        }

        public async Task<string> SearchByEAN(string ean)
        {
            dynamic fypSearchRequest = new JObject();
            fypSearchRequest.ProductId = "4719331322168";
            var jsonString = JsonConvert.SerializeObject(fypSearchRequest);

            //HttpContent httpContent = new StringContent(jsonString);
            //var jsonString = "{\"ProductId\": + " + _searchTerm + "}";
            var stringContent = new StringContent(jsonString, UnicodeEncoding.UTF8, "application/json");
            string url = "http://192.168.42.228:45456/api/Products/";

            try
            {
                var response = await httpClient.PutAsync(url, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return content;
                }
            }
            catch (Exception ex)
            {
                var t = ex;
            }

            return "-1";
        }

        public async Task<bool> UserExists(string id)
        {
            string url = "http://192.168.42.228:45455/api/User/" + id;

            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}