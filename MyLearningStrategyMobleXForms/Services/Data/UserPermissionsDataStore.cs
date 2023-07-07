using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using MyLearningStrategyMobleXForms.Models;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace MyLearningStrategyMobleXForms.Services
{
    public class UserPermissionsDataStore
    {
        private HttpClient client = new HttpClient();

        public UserPermissionsDataStore()
        {
            client.BaseAddress = new Uri($"{DataServiceConfig.ApiBaseUrl}api/UserPermissions/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }

        bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;

        //GET /api/UserPermissions
        public async Task<UserPermissionsDTO> Get(Guid Originator)
        {
            if (Originator != null && IsConnected)
            {
                HttpResponseMessage response = await client.GetAsync($"{Originator}");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var json = await response.RequestMessage.Content.ReadAsStringAsync();
                    return await Task.Run(() => JsonConvert.DeserializeObject<UserPermissionsDTO>(json));
                }
            }
            return null;
        }
        //POST /api/UserPermissions
        public async Task<UserPermissionsDTO> Post(UserPermissionsDTO permissions)
        {
            if (permissions != null && IsConnected)
            {
                var serializedItem = JsonConvert.SerializeObject(permissions);
                HttpResponseMessage response = await client.PostAsync($"", new StringContent(serializedItem, Encoding.UTF8, "application/json"));
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    var json = await response.RequestMessage.Content.ReadAsStringAsync();
                    return await Task.Run(() => JsonConvert.DeserializeObject<UserPermissionsDTO>(json));
                }
            }
            return null;
        }
        //PUT /api/UserPermissions
        public async Task<UserPermissionsDTO> Put(UserPermissionsDTO permissions)
        {
            if (permissions != null && IsConnected)
            {
                var serializedItem = JsonConvert.SerializeObject(permissions);
                HttpResponseMessage response = await client.PutAsync($"", new StringContent(serializedItem, Encoding.UTF8, "application/json"));
                if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    var json = await response.RequestMessage.Content.ReadAsStringAsync();
                    return await Task.Run(() => JsonConvert.DeserializeObject<UserPermissionsDTO>(json));
                }
            }
            return null;
        }
    }
}
