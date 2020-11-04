using System.Collections.Generic;
using System.Threading.Tasks;
using CashFlow.Model.Dto.Request;

namespace CashFlow.Services.Interfaces
{
    public interface IIncomeService
    {
        Task<IReadOnlyCollection<IncomeDto>> GetAllIncomeForUserAsync(string userId);
        Task<IncomeDto> UpdateIncomeAsync(IncomeDto income);
        Task<IncomeDto> CreateIncomeForUserAsync(IncomeDto income, string userId);
        Task RemoveIncomeAsync(string id);
    }
}
