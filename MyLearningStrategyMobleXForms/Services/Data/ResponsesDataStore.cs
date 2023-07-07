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
    public class ResponsesDataStore
    {
        private HttpClient client = new HttpClient();

        public ResponsesDataStore()
        {
            client.BaseAddress = new Uri($"{string.Concat(DataServiceConfig.ApiBaseUrl, DataServiceConfig.Responses)}");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Originator", App.CurrenUserProfile.Originator.ToString());
        }

        bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;
  
        //GET /api/Responses
        public async Task<IEnumerable<ResponsesDTO>> Get()
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
                return await Task.Run(() => JsonConvert.DeserializeObject<List<ResponsesDTO>>(response.Content.ReadAsStringAsync().Result));
            }
            
            return null;
        }
        public async Task<IEnumerable<ResponsesDTO>> GetCached()
        {
            return await BlobCache.LocalMachine.GetAndFetchLatest<IEnumerable<ResponsesDTO>>(DataServiceConfig.Responses,
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
        
        
        
        //GET /api/Responses/{ResponseId}
        public async Task<ResponsesDTO> Get(int ResponseId)
        {
            HttpResponseMessage response = await Policy
             .Handle<WebException>(ex =>
             {
                 Debug.WriteLine(ex);
                 return true;
             })
             .WaitAndRetryAsync(DataServiceConfig.Retries, retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.FromSeconds, retryAttemp)))
             .ExecuteAsync(async () => await client.GetAsync($"{ResponseId}"));

            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK && response.Content != null)
            {
                return await Task.Run(() => JsonConvert.DeserializeObject<ResponsesDTO>(response.Content.ReadAsStringAsync().Result));
            }
            return null;
        }

       
        //GET /api/Responses/BySubject/{BodyOfKnowledgeId}
        public async Task<IEnumerable<ResponsesDTO>> GetBySubject(int BodyOfKnowledgeId)
        {
            HttpResponseMessage response = await Policy
                .Handle<WebException>(ex =>
                {
                    Debug.WriteLine(ex);
                    return true;
                })
                .WaitAndRetryAsync(DataServiceConfig.Retries, retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.FromSeconds, retryAttemp)))
                .ExecuteAsync(async () => await client.GetAsync("BySubject/{BodyOfKnowledgeId}"));

            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK && response.Content != null)
            {
                return await Task.Run(() => JsonConvert.DeserializeObject<List<ResponsesDTO>>(response.Content.ReadAsStringAsync().Result));
            }

            return null;
        }
        //POST /api/Responses
        public async Task<ResponsesDTO> Post( ResponsesDTO profile)
        {
            if (profile != null)
            {
                var serializedItem = JsonConvert.SerializeObject(profile);
                HttpResponseMessage response = await Policy
                    .Handle<WebException>(ex =>
                    {
                        Debug.WriteLine(ex);
                        return true;
                    })
                    .WaitAndRetryAsync(DataServiceConfig.Retries, retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.FromSeconds, retryAttemp))
                    )
                    .ExecuteAsync(async () => await client.PostAsync(string.Empty, new StringContent(serializedItem, Encoding.UTF8, "application/json")));
                if (response != null && response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                   await BlobCache.LocalMachine.Invalidate(DataServiceConfig.Responses);
                    var json = await response.RequestMessage.Content.ReadAsStringAsync();
                    return await Task.Run(() => JsonConvert.DeserializeObject<ResponsesDTO>(json));
                }
            }
            return null;
        }
        //PUT /api/Responses
        public async Task<ResponsesDTO> Put( ResponsesDTO profile)
        {
            if (profile != null)
            {
                var serializedItem = JsonConvert.SerializeObject(profile);
                HttpResponseMessage response = await Policy
                    .Handle<WebException>(ex =>
                    {
                        Debug.WriteLine(ex);
                        return true;
                    })
                    .WaitAndRetryAsync(DataServiceConfig.Retries, retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.FromSeconds, retryAttemp)))
                    .ExecuteAsync(async () => await client.PutAsync(string.Empty, new StringContent(serializedItem, Encoding.UTF8, "application/json")));
                if (response != null && response.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    await BlobCache.LocalMachine.Invalidate(DataServiceConfig.Responses);
                    var json = await response.RequestMessage.Content.ReadAsStringAsync();
                    return await Task.Run(() => JsonConvert.DeserializeObject<ResponsesDTO>(json));
                }
            }
            return null;
        }
        //DELETE /api/Responses/{QuestionId}
        public async Task<bool> Delete( int QuestionId)
        {
            HttpResponseMessage response = await Policy
                .Handle<WebException>(ex =>
                {
                    Debug.WriteLine(ex);
                    return true;
                })
                .WaitAndRetryAsync(DataServiceConfig.Retries, retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.FromSeconds, retryAttemp)))
                .ExecuteAsync(async () => await client.DeleteAsync($"{QuestionId}"));
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                await BlobCache.LocalMachine.Invalidate(DataServiceConfig.Responses);
                return true;
            }
            return false;
        }
        //DELETE /api/Responses/{QuestionId}/{ResponseId}
        public async Task<bool> Delete( int QuestionId,int ResponseId)
        { 
            HttpResponseMessage response = await Policy
            .Handle<WebException>(ex =>
            {
                Debug.WriteLine(ex);
                return true;
            })
            .WaitAndRetryAsync(DataServiceConfig.Retries, retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.FromSeconds, retryAttemp)))
            .ExecuteAsync(async () => await client.DeleteAsync($"{QuestionId}/{ResponseId}"));

            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK) 
            {
                await BlobCache.LocalMachine.Invalidate(DataServiceConfig.Responses);
                return true; 
            }
            return false;
        }
    }
}
