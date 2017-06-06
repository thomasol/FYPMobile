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
        static HttpClient httpClient = null;

        public APIService()
        {
            httpClient =  httpClient ?? GetNewClient();
        }

        private HttpClient GetNewClient()
        {
            httpClient = new HttpClient();
            //httpClient.MaxResponseContentBufferSize = 256000;
            //httpClient.BaseAddress = new Uri("http://169.254.80.80:1234/");
            httpClient.BaseAddress = new Uri("http://sample-env.7ap3ue2fyp.eu-west-1.elasticbeanstalk.com/");
            return httpClient;
        }

        public async Task<bool> SaveEvent(JObject ev)
        {
            var jsonString = JsonConvert.SerializeObject(ev);

            var stringContent = new StringContent(jsonString, UnicodeEncoding.UTF8, "application/json");
            string url = "api/Events/";
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
            fypSearchRequest.ProductId = "555";
            var jsonString = JsonConvert.SerializeObject(fypSearchRequest);
            var stringContent = new StringContent(jsonString, UnicodeEncoding.UTF8, "application/json");
            string url = "api/Products/";

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
            string url = "api/User/" + id;

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
            string url = "api/User/" + _acct.Id;

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
            string url = "api/User/";

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

        public async Task UpdateUser(JObject ev)
        {
            var jsonString = JsonConvert.SerializeObject(ev);

            var stringContent = new StringContent(jsonString, UnicodeEncoding.UTF8, "application/json");
            string url = "api/User/";
            try
            {
                var response = await httpClient.PutAsync(url, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                Log.Debug("Update User Error", ex.ToString());
            }
        }
    }
}