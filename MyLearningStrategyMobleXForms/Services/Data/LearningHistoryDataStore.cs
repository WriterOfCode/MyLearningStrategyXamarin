using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using MyLearningStrategyMobleXForms.Models;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Polly;
using System.Net;
using System.Diagnostics;
using Akavache;
using System.Reactive.Linq;

namespace MyLearningStrategyMobleXForms.Services
{
    class LearningHistoryDataStore
    {

        private HttpClient client = new HttpClient();

        public LearningHistoryDataStore()
        {
            client.BaseAddress = new Uri($"{string.Concat(DataServiceConfig.ApiBaseUrl, DataServiceConfig.LearningHistory)}");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Originator", App.CurrenUserProfile.Originator.ToString());
        }

        bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;

        //GET /api/LearningHistory
        public async Task<IEnumerable<LearningHistoryDTO>> Get()
        {
            HttpResponseMessage response = await Policy
                .Handle<WebException>(ex =>
                {
                    Debug.WriteLine(ex);
                    return true;
                })
                .WaitAndRetryAsync(DataServiceConfig.Retries, retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.FromSeconds, retryAttemp)))
                .ExecuteAsync(async () => await client.GetAsync(String.Empty));

            if (response != null && response.StatusCode == HttpStatusCode.OK && response.Content != null)
            {
                return await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<LearningHistoryDTO>>(response.Content.ReadAsStringAsync().Result));
            }
            return null;
        }
        public async Task<IEnumerable<LearningHistoryDTO>> GetCached()
        {
            return await BlobCache.LocalMachine.GetAndFetchLatest<IEnumerable<LearningHistoryDTO>>("LearningHistoryDTO",
            () => Get(), (offset) =>
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
        //GET /api/LearningHistory/{BodyOfKnowledgeId}/{StrategyId}/{StrategyHistoryId}
        public async Task<LearningHistoryDTO> Get( int BodyOfKnowledgeId, int StrategyId, int StrategyHistoryId)
        {
            HttpResponseMessage response = await Policy
                .Handle<WebException>(ex =>
                {
                    Debug.WriteLine(ex);
                    return true;
                })
                .WaitAndRetryAsync(DataServiceConfig.Retries, retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.FromSeconds, retryAttemp)))
                .ExecuteAsync(async () => await client.GetAsync($"{BodyOfKnowledgeId}/{StrategyId}/{StrategyHistoryId}"));

            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK && response.Content != null)
            {
                return await Task.Run(() => JsonConvert.DeserializeObject<LearningHistoryDTO>(response.Content.ReadAsStringAsync().Result));
            }

            return null;
        }
        //DELETE /api/LearningHistory/{BodyOfKnowledgeId}/{StrategyId}
        public async Task<bool> Delete( int BodyOfKnowledgeId, int StrategyId)
        {
            HttpResponseMessage response = await Policy
                .Handle<WebException>(ex =>
                {
                    Debug.WriteLine(ex);
                    return true;
                })
                .WaitAndRetryAsync(DataServiceConfig.Retries, retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.FromSeconds, retryAttemp)))
                .ExecuteAsync(async () => await client.DeleteAsync($"{BodyOfKnowledgeId}/{StrategyId}"));
            if (response != null && response.StatusCode == HttpStatusCode.OK) { return true; }
            return false;
        }
        //DELETE /api/LearningHistory/{BodyOfKnowledgeId}/{StrategyId}/{StrategyHistoryId}
        public async Task<bool> Delete(int BodyOfKnowledgeId, int StrategyId, int StrategyHistoryId)
        { 
            HttpResponseMessage response = await Policy
                .Handle<WebException>(ex =>
                {
                    Debug.WriteLine(ex);
                    return true;
                })
                .WaitAndRetryAsync(DataServiceConfig.Retries, retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.FromSeconds, retryAttemp)))
                .ExecuteAsync(async () => await client.DeleteAsync($"{BodyOfKnowledgeId}/{StrategyId}/{StrategyHistoryId}"));
            if (response != null && response.StatusCode == HttpStatusCode.OK) { return true; }
            return false;
        }
        //POST /api/LearningHistory
        public async Task<LearningHistoryDTO> Post(LearningHistoryDTO LearningHistory)
        {
            try
            {
                string serializedItem = JsonConvert.SerializeObject(LearningHistory, Formatting.Indented);
                HttpResponseMessage response = await Policy
                    .Handle<WebException>(ex =>
                    {
                        Debug.WriteLine(ex);
                        return true;
                    })
                    .WaitAndRetryAsync(DataServiceConfig.Retries, retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.FromSeconds, retryAttemp))
                    )
                    .ExecuteAsync(async () => await client.PostAsync(string.Empty,
                        new StringContent(serializedItem, Encoding.UTF8, "application/json")));

                if (response != null && response.StatusCode == System.Net.HttpStatusCode.Created && response.Content != null)
                {
                    return await Task.Run(() => JsonConvert.DeserializeObject<LearningHistoryDTO>(response.Content.ReadAsStringAsync().Result));
                }
            }
            catch (Exception)
            {

                throw;
            }


            return null;
        }
        //PUT /api/LearningHistory
        public async Task<LearningHistoryDTO> Put(LearningHistoryDTO LearningHistory)
        {
            if (LearningHistory == null) { return null; }

            var serializedItem = JsonConvert.SerializeObject(LearningHistory);
            HttpResponseMessage response = await Policy
                .Handle<WebException>(ex =>
                {
                    Debug.WriteLine(ex);
                    return true;
                })
                .WaitAndRetryAsync(DataServiceConfig.Retries, retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.FromSeconds, retryAttemp))
                )
                .ExecuteAsync(async () => await client.PutAsync(string.Empty,
                new StringContent(JsonConvert.SerializeObject(LearningHistory), Encoding.UTF8, "application/json")));

            if (response != null && response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                var json = await response.RequestMessage.Content.ReadAsStringAsync();
                return await Task.Run(() => JsonConvert.DeserializeObject<LearningHistoryDTO>(json));
            }

            return null;
        }

    }
}
