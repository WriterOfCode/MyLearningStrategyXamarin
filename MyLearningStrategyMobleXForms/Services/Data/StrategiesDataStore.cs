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
    public class StrategiesDataStore
    {


        private HttpClient client = new HttpClient();
        private TimeSpan CacheExpireElapsedTime() => new TimeSpan(
                    days: DataServiceConfig.CacheExpireDays,
                    hours: DataServiceConfig.CacheExpireHour,
                    minutes: DataServiceConfig.CacheExpireMin,
                    seconds: DataServiceConfig.CacheExpireSec);

        public StrategiesDataStore()
        {
            client.BaseAddress = new Uri($"{string.Concat(DataServiceConfig.ApiBaseUrl, DataServiceConfig.Strategies)}");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Originator", App.CurrenUserProfile.Originator.ToString());
        }

        bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;

        //GET /api/Strategies/
        public async Task<IEnumerable<StrategyDTO>> Get()
        {

            HttpResponseMessage response = await Policy
                .Handle<WebException>(ex =>
                {
                    Debug.WriteLine(ex);
                    return true;
                })
                .WaitAndRetry(DataServiceConfig.Retries, retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.FromSeconds, retryAttemp)))
                .Execute(async () => await client.GetAsync(string.Empty));

            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK && response.Content != null)
            {
                return await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<StrategyDTO>>(response.Content.ReadAsStringAsync().Result));
            }
            
            return null;
        }
        public async Task<IEnumerable<StrategyDTO>> GetCached()
        {
            return await BlobCache.LocalMachine.GetAndFetchLatest("StrategyDTO",
            async () => await Get(),(offset) =>
            {
                if (Connectivity.NetworkAccess == NetworkAccess.None)
                { return false; }
                else
                {
                    TimeSpan elapsed = DateTimeOffset.Now - offset; return elapsed >
                    CacheExpireElapsedTime();
                }
            }).RunAsync(System.Threading.CancellationToken.None);
        }
        //GET /api/Strategies/{StrategyId}
        public async Task<StrategyDTO> Get(int StrategyId)
        {
            HttpResponseMessage response = await Policy
                .Handle<WebException>(ex =>
                {
                    Debug.WriteLine(ex);
                    return true;
                })
                .WaitAndRetryAsync(DataServiceConfig.Retries, retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.FromSeconds, retryAttemp)))
                .ExecuteAsync(async () => await client.GetAsync($"{StrategyId}"));

            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK && response.Content != null)
            {
                return await Task.Run(() => JsonConvert.DeserializeObject<StrategyDTO>(response.Content.ReadAsStringAsync().Result));
            }

            return null;
        }
        //PUT /api/Strategies
        public async Task<StrategyDTO> Put(StrategyDTO strategy)
        {
            if ( strategy == null) { return null; }

            var serializedItem = JsonConvert.SerializeObject(strategy);
            HttpResponseMessage response = await Policy
                .Handle<WebException>(ex =>
                {
                    Debug.WriteLine(ex);
                    return true;
                })
                .WaitAndRetryAsync(DataServiceConfig.Retries, retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.FromSeconds, retryAttemp))
                )
                .ExecuteAsync(async () => await client.PutAsync(string.Empty,
                new StringContent(JsonConvert.SerializeObject(strategy), Encoding.UTF8, "application/json")));

            if (response != null && response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                var json = await response.RequestMessage.Content.ReadAsStringAsync();
                await BlobCache.LocalMachine.Invalidate("StrategyDTO");
                return await Task.Run(() => JsonConvert.DeserializeObject<StrategyDTO>(json));
            }
            return null;
        }
        //POST /api/Strategies
        public async Task<StrategyDTO> Post(StrategyDTO strategy)
        {
            try
            {
                if (strategy == null) { return null; }

                string serializedItem = JsonConvert.SerializeObject(strategy, Formatting.Indented);
                HttpResponseMessage response = await Policy
                    .Handle<WebException>(ex =>
                    {
                        Debug.WriteLine(ex);
                        return true;
                    })
                    .WaitAndRetryAsync(DataServiceConfig.Retries, retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.FromSeconds, retryAttemp))
                    )
                    .ExecuteAsync(async () => await client.PostAsync(string.Empty ,
                        new StringContent(serializedItem, Encoding.UTF8, "application/json")));

                if (response != null && response.StatusCode == System.Net.HttpStatusCode.Created && response.Content != null)
                {
                    await BlobCache.LocalMachine.Invalidate("StrategyDTO");
                    return await Task.Run(() => JsonConvert.DeserializeObject<StrategyDTO>(response.Content.ReadAsStringAsync().Result));
                }
            }
            catch (Exception)
            {
                throw;
            }

            return null;
        }
        //DELETE /api/Strategies/{StrategyId}
        public async Task<bool> Delete( int StrategyId)
        {
            HttpResponseMessage response = await Policy
            .Handle<WebException>(ex =>
            {
                Debug.WriteLine(ex);
                return true;
            })
            .WaitAndRetryAsync(DataServiceConfig.Retries, retryAttemp => TimeSpan.FromSeconds(Math.Pow(DataServiceConfig.FromSeconds, retryAttemp)))
            .ExecuteAsync(async () => await client.DeleteAsync($"{StrategyId}"));
            
            if (response != null && response.StatusCode == HttpStatusCode.OK) 
            {
                await BlobCache.LocalMachine.Invalidate("StrategyDTO");
                return true; 
            }
            return false;
        }
    }
}
