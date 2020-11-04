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
    public class IncomeProvider : IIncomeProvider
    {
        private readonly DataContext _context;
        public IncomeProvider(DataContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyCollection<IncomeDto>> GetAllIncomeForUserAsync(string userId)
        {
            //TODO: Тест выборка
            return await _context.Incomes.Where(x => x.UserId == userId || x.UserId == null || x.UserId == string.Empty)
                .OrderBy(x => x.OrderNumber).AsNoTracking()
                .Select(x=>new IncomeDto{Id = x.Id, Name = x.Name, OrderNumber = x.OrderNumber, Price = x.Price})
                .ToArrayAsync();
        }

        public async Task<IncomeDto> UpdateIncomeAsync(IncomeDto income)
        {
            Income original = await _context.Incomes.FindAsync(income.Id);
            original.Name = income.Name;
            original.OrderNumber = income.OrderNumber;
            original.Price = income.Price;

            await _context.SaveChangesAsync();

            income.Id = original.Id;
            income.Name = original.Name;
            income.OrderNumber = original.OrderNumber;
            income.Price = original.Price;
            return income;
        }

        public async Task<IncomeDto> CreateIncomeForUserAsync(IncomeDto income, string userId)
        {
            var newIncome = new Income
            {
                Name = income.Name,
                OrderNumber = income.OrderNumber,
                Price = income.Price,
                UserId = userId
            };
            await _context.Incomes.AddAsync(newIncome);
            await _context.SaveChangesAsync();
            
            income.Id = newIncome.Id;
            income.Name = newIncome.Name;
            income.OrderNumber = newIncome.OrderNumber;
            income.Price = newIncome.Price;

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
