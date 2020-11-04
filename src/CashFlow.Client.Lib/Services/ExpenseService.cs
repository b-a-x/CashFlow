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
    public class ExpenseService : IExpenseService
    {
        private readonly HttpClient _client;

        public ExpenseService(HttpClient client)
        {
            _client = client;
        }

        public async Task<IReadOnlyCollection<ExpenseDto>> GetAllExpenseForUserAsync(string userId)
        {
            HttpResponseMessage registrationResult = await _client.GetAsync("api/expense/getallexpenseforuser");
            if (!registrationResult.IsSuccessStatusCode)
                throw new Exception(registrationResult.ToString());

            string content = await registrationResult.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IReadOnlyCollection<ExpenseDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<ExpenseDto> UpdateExpenseAsync(ExpenseDto expense)
        {
            var bodyContent = new StringContent(JsonSerializer.Serialize(expense), Encoding.UTF8, "application/json");

            var refreshResult = await _client.PutAsync("api/expense/updateexpense", bodyContent);
            if (!refreshResult.IsSuccessStatusCode)
                throw new Exception(refreshResult.ToString());

            var refreshContent = await refreshResult.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ExpenseDto>(refreshContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<ExpenseDto> CreateExpenseForUserAsync(ExpenseDto expense, string userId)
        {
            var bodyContent = new StringContent(JsonSerializer.Serialize(expense), Encoding.UTF8, "application/json");

            var refreshResult = await _client.PostAsync("api/expense/createexpenseforuser", bodyContent);
            if (!refreshResult.IsSuccessStatusCode)
                throw new Exception(refreshResult.ToString());

            var refreshContent = await refreshResult.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ExpenseDto>(refreshContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task RemoveExpenseAsync(string id)
        {
            HttpResponseMessage registrationResult = await _client.DeleteAsync($"api/expense/removeexpense?id={id}");
            if (!registrationResult.IsSuccessStatusCode)
                throw new Exception(registrationResult.ToString());
        }
    }
}
