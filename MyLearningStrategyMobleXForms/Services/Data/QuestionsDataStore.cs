using System;
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

    public class QuestionsDataStore
    {
        private HttpClient client = new HttpClient();

        public QuestionsDataStore()
        {
            client.BaseAddress = new Uri($"{DataServiceConfig.ApiBaseUrl}api/Questions/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Originator", App.CurrenUserProfile.Originator.ToString());
        }

        bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;

        //GET /api/Questions
        public async Task<IEnumerable<QuestionsDTO>> Get()
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
                return await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<QuestionsDTO>>(response.Content.ReadAsStringAsync().Result));
            }
            return null;
        }
        public async Task<IEnumerable<QuestionsDTO>> GetCached()
        {
            return await BlobCache.LocalMachine.GetAndFetchLatest<IEnumerable<QuestionsDTO>>("QuestionsDTO",
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
        //GET /api/Questions/{BodyOfKnowledgeId}/{QuestionId}
        public async Task<IEnumerable<QuestionsDTO>> Get(int BodyOfKnowledgeId,int QuestionId)
        {
            HttpResponseMessage response = await Policy
                .Handle<WebException>(ex =>
                {
                    Debug.WriteLine(ex);
                    return true;
                })
                .WaitAndRetryAsync(DataServiceConfig.Retries, retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.FromSeconds, retryAttemp)))
                .ExecuteAsync(async () => await client.GetAsync($"{BodyOfKnowledgeId}/{QuestionId}"));

            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK && response.Content != null)
            {
                return await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<QuestionsDTO>>(response.Content.ReadAsStringAsync().Result));
            }
            return null;
        }
        //POST /api/Questions
        public async Task<QuestionsDTO> Post( QuestionsDTO question)
        {
            if ( question == null) { return null; }

            string serializedItem = JsonConvert.SerializeObject(question, Formatting.Indented);
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
                await BlobCache.LocalMachine.Invalidate("QuestionsDTO");
                return await Task.Run(() => JsonConvert.DeserializeObject<QuestionsDTO>(response.Content.ReadAsStringAsync().Result));
            }

            return null;
        }
        //PUT /api/Questions
        public async Task<QuestionsDTO> Put( QuestionsDTO question)
        {
            if (question == null) { return null; }

            var serializedItem = JsonConvert.SerializeObject(question);
            HttpResponseMessage response = await Policy
                .Handle<WebException>(ex =>
                {
                    Debug.WriteLine(ex);
                    return true;
                })
                .WaitAndRetryAsync(DataServiceConfig.Retries, retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.FromSeconds, retryAttemp))
                )
                .ExecuteAsync(async () => await client.PutAsync(string.Empty,
                new StringContent(JsonConvert.SerializeObject(question), Encoding.UTF8, "application/json")));

            if (response != null && response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                await BlobCache.LocalMachine.Invalidate("QuestionsDTO");
                var json = await response.RequestMessage.Content.ReadAsStringAsync();
                return await Task.Run(() => JsonConvert.DeserializeObject<QuestionsDTO>(json));
            }

            return null;
        }
        ///DELETE /api/Questions/{BodyOfKnowledgeId}
        public async Task<bool> Delete( int BodyOfKnowledgeId)
        {
            HttpResponseMessage response = await Policy
            .Handle<WebException>(ex =>
            {
                Debug.WriteLine(ex);
                return true;
            })
            .WaitAndRetryAsync(DataServiceConfig.Retries, retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.FromSeconds, retryAttemp)))
            .ExecuteAsync(async () => await client.DeleteAsync($"{BodyOfKnowledgeId}"));

            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                await BlobCache.LocalMachine.Invalidate("QuestionsDTO");
                return true;
            }
            return false;
        }
        //DELETE /api/Questions/{BodyOfKnowledgeId}/{QuestionId}
        public async Task<bool> Delete(int BodyOfKnowledgeId, int QuestionId)
        {
            HttpResponseMessage response = await Policy
                .Handle<WebException>(ex =>
                {
                    Debug.WriteLine(ex);
                    return true;
                })
                .WaitAndRetryAsync(DataServiceConfig.Retries, retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.FromSeconds, retryAttemp)))
                .ExecuteAsync(async () => await client.DeleteAsync($"{BodyOfKnowledgeId}/{QuestionId}"));
            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                await BlobCache.LocalMachine.Invalidate("QuestionsDTO");
                return true;
            }
            return false;
        }
    }
}
