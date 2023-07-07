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
    public  class SubjectsDataStore
    {
        protected HttpClient client = new HttpClient();
        protected IBlobCache ClobCache;
        
        public SubjectsDataStore()
        {
            client.BaseAddress = new Uri($"{string.Concat(DataServiceConfig.ApiBaseUrl,DataServiceConfig.Subjects)}");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Originator", App.CurrenUserProfile.Originator.ToString());
        }

        bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;
     
        //GET /api/Subjects/
        public async Task<IEnumerable<SubjectsDTO>> Get()
        {
            if (App.CurrenUserProfile.Originator == null) { return null; }

            HttpResponseMessage response = await Policy
                .Handle<WebException>(ex =>
                {
                    Debug.WriteLine(ex);
                    return true;
                })
                .WaitAndRetryAsync(DataServiceConfig.Retries, retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.FromSeconds, retryAttemp))
                )
                .ExecuteAsync(async () => await client.GetAsync(""));
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK && response.Content != null)
            {
                return await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<SubjectsDTO>>(response.Content.ReadAsStringAsync().Result));

            }
            return null;
        }

        public async Task<IEnumerable<SubjectsDTO>> GetCached()
        {
            if (App.CurrenUserProfile.Originator == null) { return null; }
            return await BlobCache.LocalMachine.GetAndFetchLatest<IEnumerable<SubjectsDTO>>("SubjectsDTO",
            async () => await Get(), (offset) =>
            {
                if (Connectivity.NetworkAccess == NetworkAccess.None)
                { return false; }
                else
                {
                    TimeSpan elapsed = DateTimeOffset.Now - offset; return elapsed >
                    new TimeSpan(
                    hours: DataServiceConfig.CacheExpireHour,
                    minutes: DataServiceConfig.CacheExpireMin,
                    seconds: DataServiceConfig.CacheExpireSec);
                }
            }).RunAsync(System.Threading.CancellationToken.None);
        }
        //GET /api/Subjects/{BodyOfKnowledgeId}
        public async Task<IEnumerable<SubjectsDTO>> Get(int BodyOfKnowledgeId)
        {
            if (App.CurrenUserProfile.Originator == null) { return null; }

            HttpResponseMessage response = await Policy
                .Handle<WebException>(ex =>
                {
                    Debug.WriteLine(ex);
                    return true;
                })
                .WaitAndRetryAsync(DataServiceConfig.Retries, retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.FromSeconds, retryAttemp))
                )
                .ExecuteAsync(async () => await client.GetAsync("{BodyOfKnowledgeId}"));
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK && response.Content != null)
            {
                return await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<SubjectsDTO>>(response.Content.ReadAsStringAsync().Result));

            }
            return null;
        }
        //PUT /api/Subjects/
        public async Task<SubjectsDTO> Put(SubjectsDTO dto)
        {
            if (App.CurrenUserProfile.Originator == null || dto == null) { return null; }
            string serializedItem = JsonConvert.SerializeObject(dto);
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
                if (response.StatusCode == System.Net.HttpStatusCode.Accepted && response.Content != null)
                {
                    await BlobCache.LocalMachine.Invalidate("SubjectsDTO");
                    return await Task.Run(() => JsonConvert.DeserializeObject<SubjectsDTO>(response.Content.ReadAsStringAsync().Result));
                }
            }
            return null;
        }
        //POST /api/Subjects/
        public async Task<SubjectsDTO> Post(SubjectsDTO dto)
        {
            if (App.CurrenUserProfile.Originator == null || dto == null) { return null; }


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
                    await BlobCache.LocalMachine.Invalidate("SubjectsDTO");
                    var theNewSubject = await Task.Run(() => JsonConvert.DeserializeObject<SubjectsDTO>(response.Content.ReadAsStringAsync().Result));
                    if (theNewSubject == null) { theNewSubject = dto; }
                    return theNewSubject;
                }
            }

            return null;
        }
        //DELETE  /api/Subjects/{BodyOfKnowledgeId}
        public async Task<bool> Delete( int BodyOfKnowledgeId)
        {
            if (App.CurrenUserProfile.Originator == null) { return false; }

            IObservable < System.Reactive.Unit > observer = BlobCache.LocalMachine.Invalidate("SubjectsDTO");
           
           HttpResponseMessage response = await Policy
                .Handle<WebException>(ex =>
                {
                    Debug.WriteLine(ex);
                    return true;
                })
                .WaitAndRetryAsync(DataServiceConfig.Retries, retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.FromSeconds, retryAttemp))
                )
                .ExecuteAsync(async () => await client.DeleteAsync($"{BodyOfKnowledgeId}"));
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                await BlobCache.LocalMachine.Invalidate("SubjectsDTO");
                return true;
            }
            return false;
        }
    }
}
