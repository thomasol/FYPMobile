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
        

        public async Task<string> SearchByEAN(string ean)
        {
            var _searchTerm = ean;

            dynamic fypSearchRequest = new JObject();
            fypSearchRequest.ProductId = "4719331322168";
            var jsonString = JsonConvert.SerializeObject(fypSearchRequest);

            //HttpContent httpContent = new StringContent(jsonString);
            //var jsonString = "{\"ProductId\": + " + _searchTerm + "}";
            var stringContent = new StringContent(jsonString, UnicodeEncoding.UTF8, "application/json");
            string url = "http://192.168.0.164:45455/api/Products/";

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
    }
}