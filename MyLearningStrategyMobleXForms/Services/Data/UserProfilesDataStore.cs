using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using MyLearningStrategyMobleXForms.Models;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net;
using Polly;
using System.Diagnostics;
using Akavache;
using System.Reactive.Linq;

namespace MyLearningStrategyMobleXForms.Services
{
    public class UserProfilesDataStore
    {
        private HttpClient client = new HttpClient();

        public UserProfilesDataStore()
        {
            client.BaseAddress = new Uri($"{string.Concat(DataServiceConfig.ApiBaseUrl, DataServiceConfig.UserProfiles)}");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;

        //GET /api/UserProfiles/ExternalID/{ExternalID}
        public async Task<UserProfilesDTO> Get(string ExternalID)
        {
            if (ExternalID.Trim().Length == 0) { return null; }

            HttpResponseMessage response = await Policy
                .Handle<WebException>(ex =>
                {
                    Debug.WriteLine(ex);
                    return true;
                })
                .WaitAndRetryAsync(DataServiceConfig.Retries, retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.FromSeconds, retryAttemp))
                )
                .ExecuteAsync(async () => await client.GetAsync($"ExternalID/{ExternalID}"));


            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK && response.Content != null)
            {
                return await Task.Run(() => JsonConvert.DeserializeObject<UserProfilesDTO>(response.Content.ReadAsStringAsync().Result));
            }
            return null;
        }
        public async Task<UserProfilesDTO> GetCached(string ExternalID)
        {
            if (ExternalID == null) { return null; }
            return await BlobCache.Secure.GetAndFetchLatest<UserProfilesDTO>(ExternalID,
            async () => await Get(ExternalID), (offset) =>
            {
                if (Connectivity.NetworkAccess == NetworkAccess.None)
                { return false; }
                else
                {
                    TimeSpan elapsed = DateTimeOffset.Now - offset; return elapsed >
                    new TimeSpan(
                        days: DataServiceConfig.CacheExpireDays,
                    hours: DataServiceConfig.CacheExpireHour,
                    minutes: DataServiceConfig.CacheExpireMin,
                    seconds: DataServiceConfig.CacheExpireSec);
                }
            }).RunAsync(System.Threading.CancellationToken.None);
        }
        //GET /api/UserProfiles 
        public async Task<UserProfilesDTO> Get(Guid Originator)
        {
            if (Originator == null) { return null; }

            HttpResponseMessage response = await Policy
                .Handle<WebException>(ex =>
                {
                    Debug.WriteLine(ex);
                    return true;
                })
                .WaitAndRetryAsync(DataServiceConfig.Retries, retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.FromSeconds, retryAttemp))
                )
                .ExecuteAsync(async () => await client.GetAsync($"{Originator}"));


            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK && response.Content != null)
            {
                return await Task.Run(() => JsonConvert.DeserializeObject<UserProfilesDTO>(response.Content.ReadAsStringAsync().Result));

            }
            return null;
        }
        public async Task<UserProfilesDTO> GetCached(Guid Originator)
        {
            if (Originator == null) { return null; }
            return await BlobCache.Secure.GetAndFetchLatest< UserProfilesDTO > (Originator.ToString(),
            async () => await Get(Originator), (offset) =>
            {
                if (Connectivity.NetworkAccess == NetworkAccess.None)
                { return false; }
                else
                {
                    TimeSpan elapsed = DateTimeOffset.Now - offset; return elapsed >
                    new TimeSpan(
                    days: DataServiceConfig.CacheExpireDays,
                    hours: DataServiceConfig.CacheExpireHour,
                    minutes: DataServiceConfig.CacheExpireMin,
                    seconds: DataServiceConfig.CacheExpireSec);
                }
            }).RunAsync(System.Threading.CancellationToken.None);
        }
        //POST /api/UserProfiles
        public async Task<UserProfilesDTO> Post(UserProfilesDTO dto)
        {
            if (dto != null && IsConnected)
            {
                var serializedItem = JsonConvert.SerializeObject(dto);
                HttpResponseMessage response = await client.PostAsync($"", new StringContent(serializedItem, Encoding.UTF8, "application/json"));
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    var json = await response.RequestMessage.Content.ReadAsStringAsync();
                    return await Task.Run(() => JsonConvert.DeserializeObject<UserProfilesDTO>(json));
                }
            }
            return null;
        }
        //PUT /api/UserProfiles
        public async Task<UserProfilesDTO> Put(UserProfilesDTO dto)
        {
            if (dto != null && IsConnected)
            {
                var serializedItem = JsonConvert.SerializeObject(dto);
                HttpResponseMessage response = await client.PutAsync($"", new StringContent(serializedItem, Encoding.UTF8, "application/json"));
                if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    var json = await response.RequestMessage.Content.ReadAsStringAsync();
                    return await Task.Run(() => JsonConvert.DeserializeObject<UserProfilesDTO>(json));
                }
            }
            return null;
        }
    }
}