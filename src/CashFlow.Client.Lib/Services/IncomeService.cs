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
    public class IncomeService : IIncomeService
    {
        private readonly HttpClient _client;

        public IncomeService(HttpClient client)
        {
            _client = client;
        }

        public async Task<IReadOnlyCollection<IncomeDto>> GetAllIncomeForUserAsync(string userId)
        {
            HttpResponseMessage registrationResult = await _client.GetAsync("api/income/getallforuser");
            if (!registrationResult.IsSuccessStatusCode)
                throw new Exception(registrationResult.ToString());

            string content = await registrationResult.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IReadOnlyCollection<IncomeDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<IncomeDto> UpdateIncomeAsync(IncomeDto income)
        {
            var bodyContent = new StringContent(JsonSerializer.Serialize(income), Encoding.UTF8, "application/json");

            var refreshResult = await _client.PutAsync("api/income/update", bodyContent);
            if (!refreshResult.IsSuccessStatusCode)
                throw new Exception(refreshResult.ToString());

            var refreshContent = await refreshResult.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IncomeDto>(refreshContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<IncomeDto> CreateIncomeForUserAsync(IncomeDto income, string userId)
        {
            var bodyContent = new StringContent(JsonSerializer.Serialize(income), Encoding.UTF8, "application/json");

            var refreshResult = await _client.PostAsync("api/income/createforuser", bodyContent);
            if (!refreshResult.IsSuccessStatusCode)
                throw new Exception(refreshResult.ToString());

            var refreshContent = await refreshResult.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IncomeDto>(refreshContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task RemoveIncomeAsync(string id)
        {
            HttpResponseMessage registrationResult = await _client.DeleteAsync($"api/income/remove?id={id}");
            if (!registrationResult.IsSuccessStatusCode)
                throw new Exception(registrationResult.ToString());
        }
    }
}
