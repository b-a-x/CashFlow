using System.Collections.Generic;
using System.Threading.Tasks;
using CashFlow.DataProvider.Interfaces;
using CashFlow.Model.Dto.Request;
using CashFlow.Services.Interfaces;

namespace CashFlow.Server.Lib.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseProvider _provider;
        public ExpenseService(IExpenseProvider provider)
        {
            _provider = provider;
        }

        public Task<IReadOnlyCollection<ExpenseDto>> GetAllExpenseForUserAsync(string userId)
        {
            return _provider.GetAllExpenseForUserAsync(userId);
        }

        public Task<ExpenseDto> UpdateExpenseAsync(ExpenseDto expense)
        {
            return _provider.UpdateExpenseAsync(expense);
        }

        public Task<ExpenseDto> CreateExpenseForUserAsync(ExpenseDto expense, string userId)
        {
            return _provider.CreateExpenseForUserAsync(expense, userId);
        }

        public Task RemoveExpenseAsync(string id)
        {
            return _provider.RemoveExpenseAsync(id);
        }
    }
}
