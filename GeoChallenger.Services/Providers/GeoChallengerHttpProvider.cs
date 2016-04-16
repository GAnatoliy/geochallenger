using System;
using System.Net.Http;
using System.Threading.Tasks;
using GeoChallenger.Services.Providers.Interfaces;
using Newtonsoft.Json;

namespace GeoChallenger.Services.Providers
{
    public class GeoChallengerHttpProvider : IHttpProvider
    {
        private readonly HttpClient _httpClient;

        public GeoChallengerHttpProvider()
        {
            _httpClient = new HttpClient();
        }

        public async Task<TResult> HttpGetRequestAsync<TResult>(string queryUrl) where TResult : class
        {
            var httpResponse = await _httpClient.GetAsync(new Uri(queryUrl));
            if (!httpResponse.IsSuccessStatusCode) {
                return null;
            }

            var serializedAsStringResponse = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResult>(serializedAsStringResponse);
        }
    }
}
