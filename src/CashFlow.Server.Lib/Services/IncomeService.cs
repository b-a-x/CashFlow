using System.Collections.Generic;
using System.Threading.Tasks;
using CashFlow.DataProvider.Interfaces;
using CashFlow.Model.Dto.Request;
using CashFlow.Services.Interfaces;

namespace CashFlow.Server.Lib.Services
{
    public class IncomeService : IIncomeService
    {
        private readonly IIncomeProvider _provider;
        public IncomeService(IIncomeProvider provider)
        {
            _provider = provider;
        }

        public Task<IReadOnlyCollection<IncomeDto>> GetAllIncomeForUserAsync(string userId) => 
            _provider.GetAllIncomeForUserAsync(userId);

        public Task<IncomeDto> UpdateIncomeAsync(IncomeDto income) => _provider.UpdateIncomeAsync(income);

        public Task<IncomeDto> CreateIncomeAsync(IncomeDto income) => _provider.CreateIncomeAsync(income);

        public Task RemoveIncomeAsync(string id) =>
            _provider.RemoveIncomeAsync(id);
    }
}
