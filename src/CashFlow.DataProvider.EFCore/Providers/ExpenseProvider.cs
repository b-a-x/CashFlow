using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CashFlow.DataProvider.Interfaces;
using CashFlow.Model;
using CashFlow.Model.Dto.Request;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.DataProvider.EFCore.Providers
{
    //TODO: AutoMapper
    public class ExpenseProvider : IExpenseProvider
    {
        private readonly DataContext _context;
        public ExpenseProvider(DataContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyCollection<ExpenseDto>> GetAllExpenseForUserAsync(string userId)
        {
            return await _context.Expenses.Where(x => x.UserId == userId)
                .OrderBy(x => x.OrderNumber).AsNoTracking()
                .Select(x => new ExpenseDto { Id = x.Id, Name = x.Name, OrderNumber = x.OrderNumber, Price = x.Price })
                .ToArrayAsync();
        }

        public async Task<ExpenseDto> UpdateExpenseAsync(ExpenseDto expense)
        {
            Expense original = await _context.Expenses.FindAsync(expense.Id);
            original.Name = expense.Name;
            original.OrderNumber = expense.OrderNumber;
            original.Price = expense.Price;

            await _context.SaveChangesAsync();

            expense.Id = original.Id;
            expense.Name = original.Name;
            expense.OrderNumber = original.OrderNumber;
            expense.Price = original.Price;
            return expense;
        }

        public async Task<ExpenseDto> CreateExpenseForUserAsync(ExpenseDto expense, string userId)
        {
            var newExpense = new Expense
            {
                Name = expense.Name,
                OrderNumber = expense.OrderNumber,
                Price = expense.Price,
                UserId = userId
            };

            await _context.Expenses.AddAsync(newExpense);
            await _context.SaveChangesAsync();

            expense.Id = newExpense.Id;
            expense.Name = newExpense.Name;
            expense.OrderNumber = newExpense.OrderNumber;
            expense.Price = newExpense.Price;

            return expense;
        }

        public async Task RemoveExpenseAsync(string id)
        {
            Expense original = await _context.Expenses.FindAsync(id);
            _context.Expenses.Remove(original);
            await _context.SaveChangesAsync();
        }
    }
}
