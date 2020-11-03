using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CashFlow.Model.Dto.Request;

namespace CashFlow.DataProvider.Interfaces
{
    public interface IIncomeProvider
    {
        Task<IReadOnlyCollection<IncomeDto>> GetAllIncomeForUserAsync(string userId);
        Task<IncomeDto> UpdateIncomeAsync(IncomeDto income);
        Task<IncomeDto> CreateIncomeAsync(IncomeDto income);
        Task RemoveIncomeAsync(string id);
    }
}
