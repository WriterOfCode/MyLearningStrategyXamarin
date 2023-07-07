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


namespace MyLearningStrategyMobleXForms.Services.Data
{
    public class StatsDataStore
    {
        protected HttpClient client = new HttpClient();
        protected IBlobCache ClobCache;

        public StatsDataStore()
        {
            client.BaseAddress = new Uri($"{string.Concat(DataServiceConfig.ApiBaseUrl,"api/")}");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Originator", App.CurrenUserProfile.Originator.ToString());
        }

        bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;

        //GET /api/Stats/LearningHistory
        private async Task<IEnumerable<FlashCardsQuestionsDTO>> GetFlashCards( int BodyOfKnowledgeId)
        {
            HttpResponseMessage response = await Policy
                .Handle<WebException>(ex =>
                {
                    Debug.WriteLine(ex);
                    return true;
                })
                .WaitAndRetryAsync(DataServiceConfig.Retries, retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.FromSeconds, retryAttemp)))
                .ExecuteAsync(async () => await client.GetAsync($"Stats/LearningHistory/{BodyOfKnowledgeId}"));

            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK && response.Content != null)
            {
                return await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<FlashCardsQuestionsDTO>>(response.Content.ReadAsStringAsync().Result));
            }
            return null;
        }
        public async Task<IEnumerable<FlashCardsQuestionsDTO>> GetFlashCardsWithCache( int BodyOfKnowledgeId)
        {
            return await BlobCache.LocalMachine.GetAndFetchLatest<IEnumerable<FlashCardsQuestionsDTO>>("FlashCardsQuestionsDTO",
            async () => await GetFlashCards(BodyOfKnowledgeId), (offset) =>
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

        //GET /api/Stats/Questions/{BodyOfKnowledgeId}
        private async Task<IEnumerable<QuestionsStatsDTO>> GetQuestionsStats(int BodyOfKnowledgeId)
        {
            HttpResponseMessage response = await Policy
                .Handle<WebException>(ex =>
                {
                    Debug.WriteLine(ex);
                    return true;
                })
                .WaitAndRetryAsync(DataServiceConfig.Retries, retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.FromSeconds, retryAttemp)))
                .ExecuteAsync(async () => await client.GetAsync($"Stats/Questions/{BodyOfKnowledgeId}"));

            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK && response.Content != null)
            {
                return await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<QuestionsStatsDTO>>(response.Content.ReadAsStringAsync().Result));
            }
            
            return null;
        }
        public async Task<IEnumerable<QuestionsStatsDTO>> GetQuestionsStatsCached(int BodyOfKnowledgeId)
        {
            return await BlobCache.LocalMachine.GetAndFetchLatest<IEnumerable<QuestionsStatsDTO>>("QuestionsStatsDTO",
            async () => await GetQuestionsStats(BodyOfKnowledgeId), (offset) =>
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

        //GET /api/Stats/Subjects
        private async Task<IEnumerable<SubjectsStatsDTO>> GetSubjectsStats()
        {
            HttpResponseMessage response = await Policy
                .Handle<WebException>(ex =>
                {
                    Debug.WriteLine(ex);
                    return true;
                })
                .WaitAndRetryAsync(DataServiceConfig.Retries, retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.RetryMultiplyer, retryAttemp))
                )
                .ExecuteAsync(async () => await client.GetAsync($"Stats/Subjects"));
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK && response.Content != null)
            {
                IEnumerable<SubjectsStatsDTO> dtoList = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<SubjectsStatsDTO>>(response.Content.ReadAsStringAsync().Result));
                return dtoList;
            }
            return null;
        }
        public async Task<IEnumerable<SubjectsStatsDTO>> GetSubjectsStatsCached()
        {
            return await BlobCache.LocalMachine.GetAndFetchLatest<IEnumerable<SubjectsStatsDTO>>("SubjectsStatsDTO",
            async () => await GetSubjectsStats(), (offset) =>
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
    }
}
