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
    class SubjectsCategoriesDataStore
    {
        protected HttpClient client = new HttpClient();
       // protected IBlobCache ClobCache;

        public SubjectsCategoriesDataStore()
        {
            client.BaseAddress = new Uri($"{string.Concat(DataServiceConfig.ApiBaseUrl, DataServiceConfig.SubjectsCategories)}");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Originator", App.CurrenUserProfile.Originator.ToString());
        }

        bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;

        //GET /api/SubjectsCategories/
        public async Task<IEnumerable<SubjectsCategoriesDTO>> Get()
        {
            HttpResponseMessage response = await Policy
                .Handle<WebException>(ex =>
                {
                    Debug.WriteLine(ex);
                    return true;
                })
                .WaitAndRetryAsync(5, retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.FromSeconds, retryAttemp))
                )
                .ExecuteAsync(async () => await client.GetAsync(string.Empty));
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK && response.Content != null)
            {
                return await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<SubjectsCategoriesDTO>>(response.Content.ReadAsStringAsync().Result));
            }
            return null;
        }
        public async Task<IEnumerable<SubjectsCategoriesDTO>> GetCached()
        {
            return await BlobCache.LocalMachine.GetAndFetchLatest<IEnumerable<SubjectsCategoriesDTO>>("SubjectsCategoriesDTO",
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
        //GET /api/SubjectsCategories/{BodyOfKnowledgeId}/{CategoryId}
        public async Task<IEnumerable<SubjectsCategoriesDTO>> Get( int BodyOfKnowledgeId, int CategoryId)
        {
            HttpResponseMessage response = await Policy
                .Handle<WebException>(ex =>
                {
                    Debug.WriteLine(ex);
                    return true;
                })
                .WaitAndRetryAsync(DataServiceConfig.Retries, retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.FromSeconds, retryAttemp))
                )
                .ExecuteAsync(async () => await client.GetAsync($"{BodyOfKnowledgeId}/{CategoryId}"));
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK && response.Content != null)
            {
                return await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<SubjectsCategoriesDTO>>(response.Content.ReadAsStringAsync().Result));
            }
            return null;
        }
        //DELETE /api/SubjectsCategories/{BodyOfKnowledgeId}
        public async Task<bool> Delete( int BodyOfKnowledgeId)
        {
            HttpResponseMessage response = await Policy
                .Handle<WebException>(ex =>
                {
                    Debug.WriteLine(ex);
                    return true;
                })
                .WaitAndRetryAsync(DataServiceConfig.Retries, retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.FromSeconds, retryAttemp))
                )
                .ExecuteAsync(async () => await client.DeleteAsync($"{BodyOfKnowledgeId}"));
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK && response.Content != null)
            {
                await BlobCache.LocalMachine.Invalidate("SubjectsCategoriesDTO");
                return true;
            }
            return false;
        }
        //DELETE  /api/SubjectsCategories/{BodyOfKnowledgeId}/{CategoryId}
        public async Task<bool>Delete(int BodyOfKnowledgeId, int CategoryId)
        {
            HttpResponseMessage response = await Policy
                .Handle<WebException>(ex =>
                {
                    Debug.WriteLine(ex);
                    return true;
                })
                .WaitAndRetryAsync(DataServiceConfig.Retries , retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.FromSeconds, retryAttemp))
                )
                .ExecuteAsync(async () => await client.DeleteAsync($"{BodyOfKnowledgeId}/{CategoryId}"));
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK && response.Content != null)
            {
                await BlobCache.LocalMachine.Invalidate("SubjectsCategoriesDTO");
                return true;
            }
            return false;
        }
        //POST /api/SubjectsCategories
        public async Task<QuestionsDTO> Post(SubjectsCategoriesLinkDTO dto)
        {
            if (dto == null) { return null; }

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

            if (response != null && response.StatusCode == System.Net.HttpStatusCode.Created && response.Content != null)
            {
                await BlobCache.LocalMachine.Invalidate("SubjectsCategoriesDTO");
                return await Task.Run(() => JsonConvert.DeserializeObject<QuestionsDTO>(response.Content.ReadAsStringAsync().Result));
            }

            return null;
        }
    }
}
