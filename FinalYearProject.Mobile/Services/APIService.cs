using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using Android.Util;
using System.Collections.Generic;
using FinalYearProject.Domain;

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
            httpClient.BaseAddress = new Uri("http://169.254.80.80:2112/");
            //httpClient.BaseAddress = new Uri("http://sample-env.7ap3ue2fyp.eu-west-1.elasticbeanstalk.com/");
            return httpClient;
        }

        public async Task<bool> SaveEvent(JObject ev)
        {
            var jsonString = JsonConvert.SerializeObject(ev);

            var stringContent = new StringContent(jsonString, UnicodeEncoding.UTF8, "application/json");
            string url = "api/Event/";
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

        #region search

        public async Task<List<OnlineStore>> SearchByEAN(string ean)
        {
            var list = new List<OnlineStore>();
            dynamic fypSearchRequest = new JObject();
            fypSearchRequest.Mapping = "onlinestore";
            fypSearchRequest.ProductId = "05099206056213";
            fypSearchRequest.SearchType = "not text";
            var jsonString = JsonConvert.SerializeObject(fypSearchRequest);
            var stringContent = new StringContent(jsonString, UnicodeEncoding.UTF8, "application/json");
            string url = "api/Product/";

            try
            {
                var response = await httpClient.PutAsync(url, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    list = JsonConvert.DeserializeObject<List<OnlineStore>>(content);
                    return list;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("Search By EAN Error", ex.ToString());
            }
            return list;
        }

        public async Task<List<OfflineStore>> SearchByEANOffline(string ean)
        {
            var list = new List<OfflineStore>();
            dynamic fypSearchRequest = new JObject();
            fypSearchRequest.ProductId = "05099206056213";
            fypSearchRequest.Mapping = "offlinestore";
            fypSearchRequest.SearchType = "not text";

            var jsonString = JsonConvert.SerializeObject(fypSearchRequest);
            var stringContent = new StringContent(jsonString, UnicodeEncoding.UTF8, "application/json");
            string url = "api/Product/";

            try
            {
                var response = await httpClient.PutAsync(url, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    list = JsonConvert.DeserializeObject<List<OfflineStore>>(content);
                    return list;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("SearchByEAN Error", ex.ToString());
            }
            return list;
        }

        public async Task<List<OnlineStore>> SearchBySearchTerm(string term)
        {
            var list = new List<OnlineStore>();
            dynamic fypSearchRequest = new JObject();
            fypSearchRequest.Mapping = "onlinestore";
            fypSearchRequest.ProductId = "05099206056213";
            fypSearchRequest.SearchTerm = term;
            fypSearchRequest.SearchType = "text";
            var jsonString = JsonConvert.SerializeObject(fypSearchRequest);
            var stringContent = new StringContent(jsonString, UnicodeEncoding.UTF8, "application/json");
            string url = "api/Product/";

            try
            {
                var response = await httpClient.PutAsync(url, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    list = JsonConvert.DeserializeObject<List<OnlineStore>>(content);
                    return list;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("Search By Search Term Error", ex.ToString());
            }
            return list;
        }

        public async Task<List<OfflineStore>> SearchBySearchTermOffline(string term)
        {
            var list = new List<OfflineStore>();
            dynamic fypSearchRequest = new JObject();
            fypSearchRequest.ProductId = "05099206056213";
            fypSearchRequest.Mapping = "offlinestore";
            fypSearchRequest.SearchTerm = term;
            fypSearchRequest.SearchType = "text";
            var jsonString = JsonConvert.SerializeObject(fypSearchRequest);
            var stringContent = new StringContent(jsonString, UnicodeEncoding.UTF8, "application/json");
            string url = "api/Product/";

            try
            {
                var response = await httpClient.PutAsync(url, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    list = JsonConvert.DeserializeObject<List<OfflineStore>>(content);
                    return list;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("Search By Search Term Offline Error", ex.ToString());
            }
            return list;
        }

        #endregion

        #region user

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

        public async Task<User> CheckUser(string id)
        {
            User user;
            string content = "";
            string url = "api/User/" + id;

            try
            {
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    content = await response.Content.ReadAsStringAsync();
                    user = JsonConvert.DeserializeObject<User>(content);
                }
                else
                {
                    user = null;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("Check User Error", ex.ToString());
                user = null;
            }
            return user;
        }

        public async Task<User> AddUser(JObject acct)
        {
            User user;
            string content = "";
            var jsonString = JsonConvert.SerializeObject(acct);
            var stringContent = new StringContent(jsonString, UnicodeEncoding.UTF8, "application/json");
            string url = "api/User/";

            try
            {
                var response = await httpClient.PostAsync(url, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    content = await response.Content.ReadAsStringAsync();
                    user = JsonConvert.DeserializeObject<User>(content);
                }
                else
                {
                    user = null;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("AddUser Error", ex.ToString());
                user = null;
            }
            return user;
        }

        public async Task UpdateUser(JObject user)
        {
            var jsonString = JsonConvert.SerializeObject(user);

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

        public async Task<User> GetUser(string id)
        {
            User user;
            string url = "api/User/" + id;

            try
            {
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    user = JsonConvert.DeserializeObject<User>(content);
                }
                else
                {
                    user = null;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("Get User Error", ex.ToString());
                user = null;
            }
            return user;
        }
        #endregion
    }
}