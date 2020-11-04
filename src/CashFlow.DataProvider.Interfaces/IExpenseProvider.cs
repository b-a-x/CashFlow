using System.Collections.Generic;
using System.Threading.Tasks;
using CashFlow.Model.Dto.Request;

namespace CashFlow.DataProvider.Interfaces
{
    public interface IExpenseProvider
    {
        Task<IReadOnlyCollection<ExpenseDto>> GetAllExpenseForUserAsync(string userId);
        Task<ExpenseDto> UpdateExpenseAsync(ExpenseDto expense);
        Task<ExpenseDto> CreateExpenseForUserAsync(ExpenseDto expense, string userId);
        Task RemoveExpenseAsync(string id);
    }
}
