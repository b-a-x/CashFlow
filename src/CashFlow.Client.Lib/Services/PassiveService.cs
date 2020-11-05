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
    public class PassiveService : IPassiveService
    {
        private readonly HttpClient _client;

        public PassiveService(HttpClient client)
        {
            _client = client;
        }

        public async Task<IReadOnlyCollection<PassiveDto>> GetAllPassiveForUserAsync(string userId)
        {
            HttpResponseMessage registrationResult = await _client.GetAsync("api/passive/getallforuser");
            if (!registrationResult.IsSuccessStatusCode)
                throw new Exception(registrationResult.ToString());

            string content = await registrationResult.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IReadOnlyCollection<PassiveDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<PassiveDto> UpdatePassiveAsync(PassiveDto passive)
        {
            var bodyContent = new StringContent(JsonSerializer.Serialize(passive), Encoding.UTF8, "application/json");

            var refreshResult = await _client.PutAsync("api/passive/update", bodyContent);
            if (!refreshResult.IsSuccessStatusCode)
                throw new Exception(refreshResult.ToString());

            var refreshContent = await refreshResult.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PassiveDto>(refreshContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<PassiveDto> CreatePassiveForUserAsync(PassiveDto passive, string userId)
        {
            var bodyContent = new StringContent(JsonSerializer.Serialize(passive), Encoding.UTF8, "application/json");

            var refreshResult = await _client.PostAsync("api/passive/createforuser", bodyContent);
            if (!refreshResult.IsSuccessStatusCode)
                throw new Exception(refreshResult.ToString());

            var refreshContent = await refreshResult.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PassiveDto>(refreshContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task RemovePassiveAsync(string id)
        {
            HttpResponseMessage registrationResult = await _client.DeleteAsync($"api/passive/remove?id={id}");
            if (!registrationResult.IsSuccessStatusCode)
                throw new Exception(registrationResult.ToString());
        }
    }
}
