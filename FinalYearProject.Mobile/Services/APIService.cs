using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using Android.Util;
using Android.Gms.Auth.Api.SignIn;

namespace FinalYearProject.Mobile.Services
{
    public class APIService: IAPIService
    {
        HttpClient httpClient = new HttpClient();

        public APIService()
        {
            httpClient = new HttpClient();
            httpClient.MaxResponseContentBufferSize = 256000;
        }

        public async Task<bool> SaveEvent(JObject ev)
        {
            var jsonString = JsonConvert.SerializeObject(ev);

            var stringContent = new StringContent(jsonString, UnicodeEncoding.UTF8, "application/json");
            string url = "http://169.254.80.80:1234/api/Events/";
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
            string content = "";
            dynamic fypSearchRequest = new JObject();
            fypSearchRequest.ProductId = "4719331322168";
            var jsonString = JsonConvert.SerializeObject(fypSearchRequest);
            var stringContent = new StringContent(jsonString, UnicodeEncoding.UTF8, "application/json");
            string url = "http://169.254.80.80:1234/api/Products/";

            try
            {
                var response = await httpClient.PutAsync(url, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    content = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    content = "-1";
                }
            }
            catch (Exception ex)
            {
                Log.Debug("SearchByEAN Error", ex.ToString());
            }
            return content;
        }

        public async Task<bool> UserExists(string id)
        {
            string url = "http://169.254.80.80:1234/api/User/" + id;

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

        public async Task<string> CheckUser(GoogleSignInAccount _acct)
        {
            string content = "";
            string url = "http://169.254.80.80:1234/api/User/" + _acct.Id;

            try
            {
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    content = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    content = "-1";
                }
            }
            catch (Exception ex)
            {
                Log.Debug("SearchByEAN Error", ex.ToString());
            }
            return content;
        }

        public async Task<string> AddUser(JObject user)
        {
            string content = "";
            var jsonString = JsonConvert.SerializeObject(user);
            var stringContent = new StringContent(jsonString, UnicodeEncoding.UTF8, "application/json");
            string url = "http://169.254.80.80:1234/api/User/";

            try
            {
                var response = await httpClient.PostAsync(url, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    content = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    content = "-1";
                }
            }
            catch (Exception ex)
            {
                Log.Debug("AddUser Error", ex.ToString());
            }
            return content;
        }
    }
}