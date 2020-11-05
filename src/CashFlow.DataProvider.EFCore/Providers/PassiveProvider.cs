using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CashFlow.DataProvider.Interfaces;
using CashFlow.Model;
using CashFlow.Model.Dto.Request;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.DataProvider.EFCore.Providers
{
    public class PassiveProvider : IPassiveProvider
    {
        private readonly DataContext _context;
        public PassiveProvider(DataContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyCollection<PassiveDto>> GetAllPassiveForUserAsync(string userId)
        {
            //TODO: Тест выборка
            return await _context.Passives.Where(x => x.UserId == userId || x.UserId == null || x.UserId == string.Empty)
                .OrderBy(x => x.OrderNumber).AsNoTracking()
                .Select(x => new PassiveDto { Id = x.Id, Name = x.Name, OrderNumber = x.OrderNumber, Price = x.Price })
                .ToArrayAsync();
        }

        public async Task<PassiveDto> UpdatePassiveAsync(PassiveDto passive)
        {
            Passive original = await _context.Passives.FindAsync(passive.Id);
            original.Name = passive.Name;
            original.OrderNumber = passive.OrderNumber;
            original.Price = passive.Price;

            await _context.SaveChangesAsync();

            passive.Id = original.Id;
            passive.Name = original.Name;
            passive.OrderNumber = original.OrderNumber;
            passive.Price = original.Price;
            return passive;
        }

        public async Task<PassiveDto> CreatePassiveForUserAsync(PassiveDto passive, string userId)
        {
            //TODO: бизенс логику убрать в menager, сделать в одной транзакции
            //TODO: Тест выборка
            var orderNumber = _context.Expenses
                .Where(x => x.UserId == userId || x.UserId == null || x.UserId == string.Empty)
                .OrderByDescending(x => x.OrderNumber).FirstOrDefault()?.OrderNumber + 1 ?? 1;
            var newExpense = new Expense { Name = passive.Name, OrderNumber = orderNumber, Price = 0};
            await _context.Expenses.AddAsync(newExpense);

            var newPassive = new Passive
            {
                Name = passive.Name,
                OrderNumber = passive.OrderNumber,
                Price = passive.Price,
                UserId = userId,
                ExpenseId = newExpense.Id
            };
            await _context.Passives.AddAsync(newPassive);
            await _context.SaveChangesAsync();

            passive.Id = newPassive.Id;
            passive.Name = newPassive.Name;
            passive.OrderNumber = newPassive.OrderNumber;
            passive.Price = newPassive.Price;

            return passive;
        }

        public async Task RemovePassiveAsync(string id)
        {
            //TODO: бизенс логику убрать в menager, сделать в одной транзакции
            Passive original = await _context.Passives.FindAsync(id);
            Expense orign = await _context.Expenses.FindAsync(original.ExpenseId);
            _context.Passives.Remove(original);
            _context.Expenses.Remove(orign);
            await _context.SaveChangesAsync();
        }
    }
}
