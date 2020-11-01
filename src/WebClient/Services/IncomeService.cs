using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebClient.Model.Entities;

namespace WebClient.Services
{
    public interface IIncomeService
    {
        Task<IReadOnlyCollection<Income>> GetAllIncomeForUserAsync(string userId);
        Task<Income> UpdateIncomeAsync(Income income);
        Task<Income> CreateIncomeAsync(Income income);
        Task RemoveIncomeAsync(string id);
    }

    public class IncomeService : IIncomeService
    {
        private readonly HttpClient _client;

        public IncomeService(HttpClient client)
        {
            _client = client;
        }

        public async Task<IReadOnlyCollection<Income>> GetAllIncomeForUserAsync(string userId)
        {
            HttpResponseMessage registrationResult = await _client.GetAsync("api/income/getallincomeforuser");
            if (!registrationResult.IsSuccessStatusCode)
                throw new Exception(registrationResult.ToString());

            string content = await registrationResult.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IReadOnlyCollection<Income>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<Income> UpdateIncomeAsync(Income income)
        { 
            var bodyContent = new StringContent(JsonSerializer.Serialize(income), Encoding.UTF8, "application/json");

            var refreshResult = await _client.PostAsync("api/income/updateincome", bodyContent);
            if (!refreshResult.IsSuccessStatusCode)
                throw new Exception(refreshResult.ToString());

            var refreshContent = await refreshResult.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Income>(refreshContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        
        public async Task<Income> CreateIncomeAsync(Income income)
        {
            var bodyContent = new StringContent(JsonSerializer.Serialize(income), Encoding.UTF8, "application/json");

            var refreshResult = await _client.PutAsync("api/income/createincome", bodyContent);
            if (!refreshResult.IsSuccessStatusCode)
                throw new Exception(refreshResult.ToString());

            var refreshContent = await refreshResult.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Income>(refreshContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task RemoveIncomeAsync(string id)
        {
            HttpResponseMessage registrationResult = await _client.DeleteAsync($"api/income/removeincome?id={id}");
            if (!registrationResult.IsSuccessStatusCode)
                throw new Exception(registrationResult.ToString());
        }
    }
}
