using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace QuadrifoglioAPI.Services;
public class GoogleMapsService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GoogleMapsService(HttpClient httpClient, string apiKey)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;
        }

        public async Task<JObject> GetGeolocationAsync(string address)
        {
            var url = $"geocode/json?address={address}&key={_apiKey}";
            var response = await _httpClient.GetStringAsync(url);
            return JObject.Parse(response);
        }

        public async Task<JObject> GetRouteAsync(string origin, string destination)
        {
            var url = $"directions/json?origin={origin}&destination={destination}&key={_apiKey}";
            var response = await _httpClient.GetStringAsync(url);
            return JObject.Parse(response);
        }

        public async Task<JObject> GetGeolocationByLatLngAsync(string lat, string lng)
        {
            var url = $"geocode/json?latlng={lat},{lng}&key={_apiKey}";
            var response = await _httpClient.GetStringAsync(url);
            return JObject.Parse(response);
        }
}