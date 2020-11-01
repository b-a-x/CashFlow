using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CashFlowManagement.Server.Data;
using Microsoft.EntityFrameworkCore;
using WebClient.Model.Entities;
using WebClient.Services;

namespace CashFlowManagement.Server.Services
{
    public class IncomeService : IIncomeService
    {
        private readonly DataContext _context;
        public IncomeService(DataContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyCollection<Income>> GetAllIncomeForUserAsync(string userId)
        {
            //TODO: Тест выборка
            return await _context.Incomes.Where(x => x.UserId == userId || x.UserId == null || x.UserId == string.Empty)
                                         .OrderBy(x=>x.OrderNumber).AsNoTracking().ToArrayAsync();
        }

        public async Task<Income> UpdateIncomeAsync(Income income)
        {
            Income original = await _context.Incomes.FindAsync(income.Id);
            original.Name = income.Name;
            original.OrderNumber = income.OrderNumber;
            original.Price = income.Price;
            original.UserId = string.Empty; //TODO: test
            
            await _context.SaveChangesAsync();

            return original;
        }

        public async Task<Income> CreateIncomeAsync(Income income)
        {
            await _context.Incomes.AddAsync(income);
            await _context.SaveChangesAsync();
            return income;
        }

        public async Task RemoveIncomeAsync(string id)
        {
            Income original = await _context.Incomes.FindAsync(id);
            _context.Incomes.Remove(original);
            await _context.SaveChangesAsync();
        }
    }
}
