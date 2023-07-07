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
    class LearningHistoryProgressDataStore
    {
        protected HttpClient client = new HttpClient();
        protected IBlobCache ClobCache;

        public LearningHistoryProgressDataStore()
        {
            client.BaseAddress = new Uri($"{string.Concat(DataServiceConfig.ApiBaseUrl, DataServiceConfig.LearningHistoryProgress)}");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Originator", App.CurrenUserProfile.Originator.ToString());
        }

        bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;
        //GET /api/LearningHistoryProgress
        public async Task<IEnumerable<LearningHistoryProgressDTO>> Get()
        {
            HttpResponseMessage response = await Policy
                .Handle<WebException>(ex =>
                {
                    Debug.WriteLine(ex);
                    return true;
                })
                .WaitAndRetryAsync(DataServiceConfig.Retries, retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.FromSeconds, retryAttemp)))
                .ExecuteAsync(async () => await client.GetAsync(string.Empty));

            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK && response.Content != null)
            {
                return await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<LearningHistoryProgressDTO>>(response.Content.ReadAsStringAsync().Result));
            }
            return null;
        }
        public async Task<IEnumerable<LearningHistoryProgressDTO>> GetCached()
        {
            return await BlobCache.LocalMachine.GetAndFetchLatest<IEnumerable<LearningHistoryProgressDTO>>("LearningHistoryDTO",
            async () => await Get(), (offset) =>
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
        //GET /api/LearningHistoryProgress/{BodyOfKnowledgeId}/{StrategyId}/{StrategyHistoryId}
        private async Task<IEnumerable<LearningHistoryProgressDTO>> Get(int BodyOfKnowledgeId, int StrategyId, int StrategyHistoryId)
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
                return await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<LearningHistoryProgressDTO>>(response.Content.ReadAsStringAsync().Result));
            }
            return null;
        }
        public async Task<IEnumerable<LearningHistoryProgressDTO>> GetCached( int BodyOfKnowledgeId, int StrategyId, int StrategyHistoryId)
        { 
            return await BlobCache.LocalMachine.GetAndFetchLatest<IEnumerable<LearningHistoryProgressDTO>>("LearningHistoryProgressDTO",
            async () => await Get(), (offset) =>
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
        //POST /api/LearningHistoryProgress
        public async Task<LearningHistoryProgressDTO> Post(LearningHistoryProgressDTO dto)
        {
            if ( dto == null) { return null; }
            string serializedItem = JsonConvert.SerializeObject(dto, Formatting.Indented);

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

            if (response != null)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Created && response.Content != null)
                {
                    return await Task.Run(() => JsonConvert.DeserializeObject<LearningHistoryProgressDTO>(response.Content.ReadAsStringAsync().Result));

                }
            }
            return null;
        }
        //PUT /api/LearningHistoryProgress
        public async Task<LearningHistoryProgressDTO> Put( LearningHistoryProgressDTO dto)
        {
            if (  dto == null) { return null; }
            string serializedItem = JsonConvert.SerializeObject(dto, Formatting.Indented);

            HttpResponseMessage response = await Policy
                .Handle<WebException>(ex =>
                {
                    Debug.WriteLine(ex);
                    return true;
                })
                .WaitAndRetryAsync(DataServiceConfig.Retries, retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.FromSeconds, retryAttemp))
                )
                .ExecuteAsync(async () => await client.PutAsync(string.Empty,
                new StringContent(serializedItem, Encoding.UTF8, "application/json")));

            if (response != null)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Created && response.Content != null)
                {
                    return await Task.Run(() => JsonConvert.DeserializeObject<LearningHistoryProgressDTO>(response.Content.ReadAsStringAsync().Result));

                }
            }
            return null;
        }
        //PATCH /api/LearningHistoryProgress
    }
}
