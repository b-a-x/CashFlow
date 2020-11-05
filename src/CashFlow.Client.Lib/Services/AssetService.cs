using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CashFlow.Model.Dto.Request;
using CashFlow.Services.Interfaces;

namespace CashFlow.Client.Lib.Services
{
    public class AssetService : IAssetService
    {
        private readonly HttpClient _client;

        public AssetService(HttpClient client)
        {
            _client = client;
        }

        public async Task<IReadOnlyCollection<AssetDto>> GetAllAssetForUserAsync(string userId)
        {
            HttpResponseMessage registrationResult = await _client.GetAsync("api/asset/getallforuser");
            if (!registrationResult.IsSuccessStatusCode)
                throw new Exception(registrationResult.ToString());

            string content = await registrationResult.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IReadOnlyCollection<AssetDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<AssetDto> UpdateAssetAsync(AssetDto asset)
        {
            var bodyContent = new StringContent(JsonSerializer.Serialize(asset), Encoding.UTF8, "application/json");

            var refreshResult = await _client.PutAsync("api/asset/update", bodyContent);
            if (!refreshResult.IsSuccessStatusCode)
                throw new Exception(refreshResult.ToString());

            var refreshContent = await refreshResult.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AssetDto>(refreshContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<AssetDto> CreateAssetForUserAsync(AssetDto asset, string userId)
        {
            var bodyContent = new StringContent(JsonSerializer.Serialize(asset), Encoding.UTF8, "application/json");

            var refreshResult = await _client.PostAsync("api/asset/createforuser", bodyContent);
            if (!refreshResult.IsSuccessStatusCode)
                throw new Exception(refreshResult.ToString());

            var refreshContent = await refreshResult.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AssetDto>(refreshContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task RemoveAssetAsync(string id)
        {
            HttpResponseMessage registrationResult = await _client.DeleteAsync($"api/asset/remove?id={id}");
            if (!registrationResult.IsSuccessStatusCode)
                throw new Exception(registrationResult.ToString());
        }
    }
}
